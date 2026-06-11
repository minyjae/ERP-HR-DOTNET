using ERP.Application.Common.Exceptions;
using ERP.Application.DTOs;
using ERP.Application.Interfaces;
using ERP.Application.Mappings;
using ERP.Domain.Entities;
using FluentValidation;

namespace ERP.Application.Services;

/// <summary>
/// business logic ฝั่งโควต้าวันลา: validate → ตรวจกฎ (โควต้าซ้ำ) → เรียก domain → repo บันทึก
/// การหัก/คืนโควต้า (Consume/Release) จะถูกเรียกจากโมดูล LeaveRequest ตอนอนุมัติ/ยกเลิก ไม่ใช่ที่นี่
/// </summary>
public class LeaveAllocationService(
    ILeaveAllocationRepository repository,
    IValidator<CreateLeaveAllocationRequest> createValidator,
    IValidator<UpdateLeaveAllocationRequest> updateValidator) : ILeaveAllocationService
{
    public async Task<List<LeaveAllocationResponse>> GetByEmployeeAsync(Guid employeeId, int? year, CancellationToken ct = default)
    {
        var items = await repository.GetByEmployeeAsync(employeeId, year, ct);
        return items.Select(la => la.ToResponse()).ToList();
    }

    public async Task<LeaveAllocationResponse?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var allocation = await repository.GetByIdAsync(id, ct);
        return allocation?.ToResponse();
    }

    public async Task<LeaveAllocationResponse> CreateAsync(CreateLeaveAllocationRequest request, CancellationToken ct = default)
    {
        await createValidator.ValidateAndThrowAsync(request, ct);

        // กฎทางธุรกิจ: พนักงาน 1 คน มีโควต้าต่อ (ประเภทลา, ปี) ได้ทีละ 1
        if (await repository.ExistsAsync(request.EmployeeId, request.LeaveTypeId, request.Year, ct))
            throw new ConflictException("พนักงานคนนี้มีโควต้าของประเภทลานี้ในปีดังกล่าวอยู่แล้ว");

        var allocation = LeaveAllocation.Create(
            employeeId: request.EmployeeId,
            leaveTypeId: request.LeaveTypeId,
            year: request.Year,
            totalDays: request.TotalDays,
            carryOverDays: request.CarryOverDays);

        await repository.AddAsync(allocation, ct);
        await repository.SaveChangesAsync(ct);

        return allocation.ToResponse();
    }

    public async Task<LeaveAllocationResponse> UpdateAsync(Guid id, UpdateLeaveAllocationRequest request, CancellationToken ct = default)
    {
        await updateValidator.ValidateAndThrowAsync(request, ct);

        var allocation = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"ไม่พบโควต้าวันลา id '{id}'");

        allocation.Adjust(request.TotalDays, request.CarryOverDays);
        await repository.SaveChangesAsync(ct);
        return allocation.ToResponse();
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var allocation = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"ไม่พบโควต้าวันลา id '{id}'");

        repository.Remove(allocation);
        await repository.SaveChangesAsync(ct);
    }
}
