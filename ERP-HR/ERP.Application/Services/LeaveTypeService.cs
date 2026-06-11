using ERP.Application.Common.Exceptions;
using ERP.Application.DTOs;
using ERP.Application.Interfaces;
using ERP.Application.Mappings;
using ERP.Domain.Entities;
using FluentValidation;

namespace ERP.Application.Services;

/// <summary>
/// business logic ฝั่ง LeaveType: validate → ตรวจกฎ (Code ซ้ำ) → เรียก domain → repo บันทึก
/// </summary>
public class LeaveTypeService(
    ILeaveTypeRepository repository,
    IValidator<CreateLeaveTypeRequest> createValidator,
    IValidator<UpdateLeaveTypeRequest> updateValidator) : ILeaveTypeService
{
    public async Task<List<LeaveTypeResponse>> GetAllAsync(CancellationToken ct = default)
    {
        var leaveTypes = await repository.GetAllAsync(ct);
        return leaveTypes.Select(lt => lt.ToResponse()).ToList();
    }

    public async Task<LeaveTypeResponse?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var leaveType = await repository.GetByIdAsync(id, ct);
        return leaveType?.ToResponse();
    }

    public async Task<LeaveTypeResponse> CreateAsync(CreateLeaveTypeRequest request, CancellationToken ct = default)
    {
        await createValidator.ValidateAndThrowAsync(request, ct);

        // กฎทางธุรกิจ: รหัสประเภทลาห้ามซ้ำ
        if (await repository.ExistsByCodeAsync(request.Code, ct))
            throw new ConflictException($"รหัสประเภทลา '{request.Code}' ถูกใช้งานแล้ว");

        var leaveType = LeaveType.Create(
            code: request.Code,
            name: request.Name,
            isPaid: request.IsPaid,
            requiresDocument: request.RequiresDocument);

        await repository.AddAsync(leaveType, ct);
        await repository.SaveChangesAsync(ct);

        return leaveType.ToResponse();
    }

    public async Task<LeaveTypeResponse> UpdateAsync(Guid id, UpdateLeaveTypeRequest request, CancellationToken ct = default)
    {
        await updateValidator.ValidateAndThrowAsync(request, ct);

        var leaveType = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"ไม่พบประเภทลา id '{id}'");

        leaveType.UpdateDetails(
            name: request.Name,
            isPaid: request.IsPaid,
            requiresDocument: request.RequiresDocument);

        await repository.SaveChangesAsync(ct);
        return leaveType.ToResponse();
    }

    public async Task ActivateAsync(Guid id, CancellationToken ct = default)
    {
        var leaveType = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"ไม่พบประเภทลา id '{id}'");

        leaveType.Activate();
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeactivateAsync(Guid id, CancellationToken ct = default)
    {
        var leaveType = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"ไม่พบประเภทลา id '{id}'");

        leaveType.Deactivate();
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var leaveType = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"ไม่พบประเภทลา id '{id}'");

        repository.Remove(leaveType);
        await repository.SaveChangesAsync(ct);
    }
}
