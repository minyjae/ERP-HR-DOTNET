using ERP.Application.Common.Exceptions;
using ERP.Application.DTOs;
using ERP.Application.Interfaces;
using ERP.Application.Mappings;
using ERP.Domain.Entities;
using FluentValidation;

namespace ERP.Application.Services;

/// <summary>
/// business logic ฝั่งนโยบายการลา
/// กฎหลัก: 1 LeaveType มี policy ที่ active ได้ทีละ 1 (ตรวจตอน Create และ Activate)
/// </summary>
public class LeavePolicyService(
    ILeavePolicyRepository repository,
    IValidator<CreateLeavePolicyRequest> createValidator,
    IValidator<UpdateLeavePolicyRequest> updateValidator) : ILeavePolicyService
{
    public async Task<List<LeavePolicyResponse>> GetAllAsync(CancellationToken ct = default)
    {
        var policies = await repository.GetAllAsync(ct);
        return policies.Select(p => p.ToResponse()).ToList();
    }

    public async Task<List<LeavePolicyResponse>> GetByLeaveTypeAsync(Guid leaveTypeId, CancellationToken ct = default)
    {
        var policies = await repository.GetByLeaveTypeAsync(leaveTypeId, ct);
        return policies.Select(p => p.ToResponse()).ToList();
    }

    public async Task<LeavePolicyResponse?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var policy = await repository.GetByIdAsync(id, ct);
        return policy?.ToResponse();
    }

    public async Task<LeavePolicyResponse> CreateAsync(CreateLeavePolicyRequest request, CancellationToken ct = default)
    {
        await createValidator.ValidateAndThrowAsync(request, ct);

        // กฎ: ห้ามมี policy ที่ active ซ้อนกันในประเภทลาเดียวกัน
        if (await repository.HasActivePolicyAsync(request.LeaveTypeId, null, ct))
            throw new ConflictException("ประเภทลานี้มีนโยบายที่ใช้งานอยู่แล้ว (active ได้ทีละ 1)");

        var policy = LeavePolicy.Create(
            leaveTypeId: request.LeaveTypeId,
            entitledDays: request.EntitledDays,
            maxCarryOverDays: request.MaxCarryOverDays,
            minServiceMonths: request.MinServiceMonths,
            advanceNoticeDays: request.AdvanceNoticeDays);

        await repository.AddAsync(policy, ct);
        await repository.SaveChangesAsync(ct);

        return policy.ToResponse();
    }

    public async Task<LeavePolicyResponse> UpdateAsync(Guid id, UpdateLeavePolicyRequest request, CancellationToken ct = default)
    {
        await updateValidator.ValidateAndThrowAsync(request, ct);

        var policy = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"ไม่พบนโยบายการลา id '{id}'");

        policy.UpdateDetails(
            entitledDays: request.EntitledDays,
            maxCarryOverDays: request.MaxCarryOverDays,
            minServiceMonths: request.MinServiceMonths,
            advanceNoticeDays: request.AdvanceNoticeDays);

        await repository.SaveChangesAsync(ct);
        return policy.ToResponse();
    }

    public async Task ActivateAsync(Guid id, CancellationToken ct = default)
    {
        var policy = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"ไม่พบนโยบายการลา id '{id}'");

        // กันเปิดใช้ซ้อนกับ policy active อื่นในประเภทลาเดียวกัน
        if (await repository.HasActivePolicyAsync(policy.LeaveTypeId, policy.Id, ct))
            throw new ConflictException("ประเภทลานี้มีนโยบายที่ใช้งานอยู่แล้ว (active ได้ทีละ 1)");

        policy.Activate();
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeactivateAsync(Guid id, CancellationToken ct = default)
    {
        var policy = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"ไม่พบนโยบายการลา id '{id}'");

        policy.Deactivate();
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var policy = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"ไม่พบนโยบายการลา id '{id}'");

        repository.Remove(policy);
        await repository.SaveChangesAsync(ct);
    }
}
