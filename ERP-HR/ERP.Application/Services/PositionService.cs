using ERP.Application.Common.Exceptions;
using ERP.Application.DTOs;
using ERP.Application.Interfaces;
using ERP.Application.Mappings;
using ERP.Domain.Entities;
using FluentValidation;

namespace ERP.Application.Services;

/// <summary>
/// business logic ฝั่ง Position: validate → ตรวจกฎ (Code ซ้ำ) → เรียก domain → สั่ง repo บันทึก
/// service ไม่รู้จัก HTTP และไม่รู้จัก EF Core — รับ/ส่งเป็น DTO กับ entity เท่านั้น
/// </summary>
public class PositionService(
    IPositionRepository repository,
    IValidator<CreatePositionRequest> createValidator,
    IValidator<UpdatePositionRequest> updateValidator) : IPositionService
{
    public async Task<List<PositionResponse>> GetAllAsync(CancellationToken ct = default)
    {
        var positions = await repository.GetAllAsync(ct);
        return positions.Select(p => p.ToResponse()).ToList();
    }

    public async Task<PositionResponse?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var position = await repository.GetByIdAsync(id, ct);
        return position?.ToResponse();
    }

    public async Task<PositionResponse> CreateAsync(CreatePositionRequest request, CancellationToken ct = default)
    {
        // 1) ตรวจรูปแบบข้อมูล (โยน ValidationException → 400)
        await createValidator.ValidateAndThrowAsync(request, ct);

        // 2) กฎทางธุรกิจที่ต้องดู database: รหัสตำแหน่งห้ามซ้ำ
        if (await repository.ExistsByCodeAsync(request.Code, ct))
            throw new ConflictException($"รหัสตำแหน่ง '{request.Code}' ถูกใช้งานแล้ว");

        // 3) สร้าง entity ผ่าน factory ของ domain
        var position = Position.Create(
            code: request.Code,
            name: request.Name,
            level: request.Level,
            nameEn: request.NameEn,
            departmentId: request.DepartmentId);

        // 4) บันทึก
        await repository.AddAsync(position, ct);
        await repository.SaveChangesAsync(ct);

        return position.ToResponse();
    }

    public async Task<PositionResponse> UpdateAsync(Guid id, UpdatePositionRequest request, CancellationToken ct = default)
    {
        await updateValidator.ValidateAndThrowAsync(request, ct);

        var position = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"ไม่พบตำแหน่ง id '{id}'");

        position.UpdateDetails(
            name: request.Name,
            level: request.Level,
            nameEn: request.NameEn,
            departmentId: request.DepartmentId);

        await repository.SaveChangesAsync(ct);
        return position.ToResponse();
    }

    public async Task ActivateAsync(Guid id, CancellationToken ct = default)
    {
        var position = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"ไม่พบตำแหน่ง id '{id}'");

        position.Activate();
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeactivateAsync(Guid id, CancellationToken ct = default)
    {
        var position = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"ไม่พบตำแหน่ง id '{id}'");

        position.Deactivate();
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var position = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"ไม่พบตำแหน่ง id '{id}'");

        repository.Remove(position);
        await repository.SaveChangesAsync(ct);
    }
}
