using ERP.Application.Common.Exceptions;
using ERP.Application.DTOs;
using ERP.Application.Interfaces;
using ERP.Application.Mappings;
using ERP.Domain.Entities;
using FluentValidation;

namespace ERP.Application.Services;

/// <summary>
/// business logic ฝั่ง Branch: validate → ตรวจกฎ (Code ซ้ำ) → เรียก domain → สั่ง repo บันทึก
/// service ไม่รู้จัก HTTP และไม่รู้จัก EF Core — รับ/ส่งเป็น DTO กับ entity เท่านั้น
/// </summary>
public class BranchService(
    IBranchRepository repository,
    IValidator<CreateBranchRequest> createValidator,
    IValidator<UpdateBranchRequest> updateValidator) : IBranchService
{
    public async Task<List<BranchResponse>> GetAllAsync(CancellationToken ct = default)
    {
        var branches = await repository.GetAllAsync(ct);
        return branches.Select(b => b.ToResponse()).ToList();
    }

    public async Task<BranchResponse?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var branch = await repository.GetByIdAsync(id, ct);
        return branch?.ToResponse();
    }

    public async Task<BranchResponse> CreateAsync(CreateBranchRequest request, CancellationToken ct = default)
    {
        // 1) ตรวจรูปแบบข้อมูล (โยน ValidationException → 400)
        await createValidator.ValidateAndThrowAsync(request, ct);

        // 2) กฎทางธุรกิจที่ต้องดู database: รหัสสาขาห้ามซ้ำ
        if (await repository.ExistsByCodeAsync(request.Code, ct))
            throw new ConflictException($"รหัสสาขา '{request.Code}' ถูกใช้งานแล้ว");

        // 3) สร้าง entity ผ่าน factory ของ domain
        var branch = Branch.Create(
            code: request.Code,
            name: request.Name,
            address: request.Address,
            phone: request.Phone);

        // 4) บันทึก
        await repository.AddAsync(branch, ct);
        await repository.SaveChangesAsync(ct);

        return branch.ToResponse();
    }

    public async Task<BranchResponse> UpdateAsync(Guid id, UpdateBranchRequest request, CancellationToken ct = default)
    {
        await updateValidator.ValidateAndThrowAsync(request, ct);

        var branch = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"ไม่พบสาขา id '{id}'");

        branch.UpdateDetails(
            name: request.Name,
            address: request.Address,
            phone: request.Phone);

        await repository.SaveChangesAsync(ct);
        return branch.ToResponse();
    }

    public async Task ActivateAsync(Guid id, CancellationToken ct = default)
    {
        var branch = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"ไม่พบสาขา id '{id}'");

        branch.Activate();
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeactivateAsync(Guid id, CancellationToken ct = default)
    {
        var branch = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"ไม่พบสาขา id '{id}'");

        branch.Deactivate();
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var branch = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"ไม่พบสาขา id '{id}'");

        repository.Remove(branch);
        await repository.SaveChangesAsync(ct);
    }
}
