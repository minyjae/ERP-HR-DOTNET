using ERP.Application.Common.Exceptions;
using ERP.Application.DTOs;
using ERP.Application.Interfaces;
using ERP.Application.Mappings;
using ERP.Domain.Entities;
using ERP.Domain.Enums;
using FluentValidation;

namespace ERP.Application.Services;

/// <summary>
/// business logic ฝั่งใบลา — orchestrate หลายโมดูลเข้าด้วยกัน:
/// Holiday (คำนวณวันลา), LeaveType (บังคับเอกสาร), LeavePolicy (แจ้งล่วงหน้า), LeaveAllocation (โควต้า)
/// </summary>
public class LeaveRequestService(
    ILeaveRequestRepository repository,
    ILeaveTypeRepository leaveTypeRepository,
    ILeavePolicyRepository policyRepository,
    ILeaveAllocationRepository allocationRepository,
    IHolidayRepository holidayRepository,
    IValidator<CreateLeaveRequestRequest> createValidator,
    IValidator<ApproveLeaveRequestRequest> approveValidator,
    IValidator<RejectLeaveRequestRequest> rejectValidator) : ILeaveRequestService
{
    public async Task<List<LeaveRequestResponse>> GetByEmployeeAsync(
        Guid employeeId, LeaveRequestStatus? status, int? year, CancellationToken ct = default)
    {
        var items = await repository.GetByEmployeeAsync(employeeId, status, year, ct);
        return items.Select(r => r.ToResponse()).ToList();
    }

    public async Task<LeaveRequestResponse?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var request = await repository.GetByIdAsync(id, ct);
        return request?.ToResponse();
    }

    public async Task<LeaveRequestResponse> CreateAsync(CreateLeaveRequestRequest request, CancellationToken ct = default)
    {
        await createValidator.ValidateAndThrowAsync(request, ct);

        // 1) ประเภทลาต้องมีอยู่จริง
        var leaveType = await leaveTypeRepository.GetByIdAsync(request.LeaveTypeId, ct)
            ?? throw new NotFoundException($"ไม่พบประเภทลา id '{request.LeaveTypeId}'");

        // 2) บังคับแนบเอกสารถ้าประเภทลานั้นกำหนด
        if (leaveType.RequiresDocument && string.IsNullOrWhiteSpace(request.AttachmentUrl))
            throw new ValidationException("ประเภทลานี้ต้องแนบเอกสารประกอบ");

        // 3) ห้ามช่วงวันลาทับกับใบลา Pending/Approved เดิม
        if (await repository.HasOverlapAsync(request.EmployeeId, request.StartDate, request.EndDate, null, ct))
            throw new ConflictException("ช่วงวันลานี้ทับซ้อนกับใบลาที่มีอยู่");

        // 4) คำนวณจำนวนวันลาจริง (หักเสาร์-อาทิตย์ + วันหยุด)
        var holidays = await holidayRepository.GetBetweenAsync(request.StartDate, request.EndDate, ct);
        var holidaySet = holidays.Select(h => h.Date).ToHashSet();
        var totalDays = CountWorkingDays(request.StartDate, request.EndDate, holidaySet);
        if (totalDays <= 0)
            throw new ValidationException("ช่วงวันที่เลือกไม่มีวันทำงาน (เป็นวันหยุดทั้งหมด)");

        // 5) บังคับแจ้งล่วงหน้าตามนโยบาย (ถ้ามี policy ที่ active)
        var policy = await policyRepository.GetActiveByLeaveTypeAsync(request.LeaveTypeId, ct);
        if (policy is not null && policy.AdvanceNoticeDays > 0)
        {
            var earliest = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(policy.AdvanceNoticeDays);
            if (request.StartDate < earliest)
                throw new ConflictException($"ต้องยื่นลาล่วงหน้าอย่างน้อย {policy.AdvanceNoticeDays} วัน");
        }

        // 6) ตรวจโควต้าคงเหลือ (ยังไม่หัก — หักตอนอนุมัติ)
        var allocation = await allocationRepository.GetByKeyAsync(request.EmployeeId, request.LeaveTypeId, request.StartDate.Year, ct)
            ?? throw new ConflictException("ยังไม่มีโควต้าวันลาของประเภทนี้ในปีดังกล่าว");
        if (allocation.RemainingDays < totalDays)
            throw new ConflictException($"วันลาคงเหลือไม่พอ (เหลือ {allocation.RemainingDays} ขอ {totalDays})");

        var leaveRequest = LeaveRequest.Create(
            employeeId: request.EmployeeId,
            leaveTypeId: request.LeaveTypeId,
            startDate: request.StartDate,
            endDate: request.EndDate,
            totalDays: totalDays,
            reason: request.Reason,
            attachmentUrl: request.AttachmentUrl);

        await repository.AddAsync(leaveRequest, ct);
        await repository.SaveChangesAsync(ct);

        return leaveRequest.ToResponse();
    }

    public async Task<LeaveRequestResponse> ApproveAsync(Guid id, ApproveLeaveRequestRequest request, CancellationToken ct = default)
    {
        await approveValidator.ValidateAndThrowAsync(request, ct);

        var leaveRequest = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"ไม่พบใบลา id '{id}'");

        if (leaveRequest.Status != LeaveRequestStatus.Pending)
            throw new ConflictException($"อนุมัติใบลาสถานะ {leaveRequest.Status} ไม่ได้");

        // หักโควต้า ณ ตอนอนุมัติ
        var allocation = await allocationRepository.GetByKeyAsync(
                leaveRequest.EmployeeId, leaveRequest.LeaveTypeId, leaveRequest.StartDate.Year, ct)
            ?? throw new ConflictException("ไม่พบโควต้าวันลาสำหรับหัก");

        var days = (int)leaveRequest.TotalDays;
        if (allocation.RemainingDays < days)
            throw new ConflictException($"วันลาคงเหลือไม่พอ (เหลือ {allocation.RemainingDays} ขอ {days})");

        allocation.Consume(days);
        leaveRequest.Approve(request.ApproverId);

        await repository.SaveChangesAsync(ct);   // allocation + request commit พร้อมกัน (atomic)
        return leaveRequest.ToResponse();
    }

    public async Task<LeaveRequestResponse> RejectAsync(Guid id, RejectLeaveRequestRequest request, CancellationToken ct = default)
    {
        await rejectValidator.ValidateAndThrowAsync(request, ct);

        var leaveRequest = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"ไม่พบใบลา id '{id}'");

        if (leaveRequest.Status != LeaveRequestStatus.Pending)
            throw new ConflictException($"ปฏิเสธใบลาสถานะ {leaveRequest.Status} ไม่ได้");

        leaveRequest.Reject(request.ApproverId, request.RejectReason);
        await repository.SaveChangesAsync(ct);
        return leaveRequest.ToResponse();
    }

    public async Task CancelAsync(Guid id, CancellationToken ct = default)
    {
        var leaveRequest = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"ไม่พบใบลา id '{id}'");

        if (leaveRequest.Status is not (LeaveRequestStatus.Pending or LeaveRequestStatus.Approved))
            throw new ConflictException($"ยกเลิกใบลาสถานะ {leaveRequest.Status} ไม่ได้");

        var wasApproved = leaveRequest.Status == LeaveRequestStatus.Approved;
        leaveRequest.Cancel();

        // ถ้าเคยอนุมัติแล้ว ต้องคืนโควต้าที่หักไป
        if (wasApproved)
        {
            var allocation = await allocationRepository.GetByKeyAsync(
                leaveRequest.EmployeeId, leaveRequest.LeaveTypeId, leaveRequest.StartDate.Year, ct);
            allocation?.Release((int)leaveRequest.TotalDays);
        }

        await repository.SaveChangesAsync(ct);
    }

    /// <summary>นับวันทำงานจริงในช่วง [start, end] — ตัดเสาร์-อาทิตย์และวันหยุดออก</summary>
    private static decimal CountWorkingDays(DateOnly start, DateOnly end, HashSet<DateOnly> holidays)
    {
        decimal days = 0;
        for (var d = start; d <= end; d = d.AddDays(1))
        {
            if (d.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday) continue;
            if (holidays.Contains(d)) continue;
            days++;
        }
        return days;
    }
}
