using ERP.Application.Common.Exceptions;
using ERP.Application.DTOs;
using ERP.Application.Interfaces;
using ERP.Application.Mappings;
using ERP.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace ERP.Application.Services;

/// <summary>
/// business logic ฝั่ง Department: validate → ตรวจกฎ (Code ซ้ำ / กันเป็นแม่ตัวเอง) → เรียก domain → สั่ง repo บันทึก
/// service ไม่รู้จัก HTTP และไม่รู้จัก EF Core — รับ/ส่งเป็น DTO กับ entity เท่านั้น
/// </summary>
public class DepartmentService(
    IDepartmentRepository repository,
    IValidator<CreateDepartmentRequest> createValidator,
    IValidator<UpdateDepartmentRequest> updateValidator) : IDepartmentService
{
    public async Task<List<DepartmentResponse>> GetAllAsync(CancellationToken ct = default)
    {
        var departments = await repository.GetAllAsync(ct);
        return departments.Select(d => d.ToResponse()).ToList();
    }

    public async Task<DepartmentResponse?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var department = await repository.GetByIdAsync(id, ct);
        return department?.ToResponse();
    }

    public async Task<DepartmentResponse> CreateAsync(CreateDepartmentRequest request, CancellationToken ct = default)
    {
        // 1) ตรวจรูปแบบข้อมูล (โยน ValidationException → 400)
        await createValidator.ValidateAndThrowAsync(request, ct);

        // 2) กฎทางธุรกิจที่ต้องดู database: รหัสแผนกห้ามซ้ำ
        if (await repository.ExistsByCodeAsync(request.Code, ct))
            throw new ConflictException($"รหัสแผนก '{request.Code}' ถูกใช้งานแล้ว");

        // 3) สร้าง entity ผ่าน factory ของ domain
        var department = Department.Create(
            code: request.Code,
            name: request.Name,
            nameEn: request.NameEn,
            managerId: request.ManagerId,
            parentDepartmentId: request.ParentDepartmentId);

        // 4) บันทึก
        await repository.AddAsync(department, ct);
        await repository.SaveChangesAsync(ct);

        return department.ToResponse();
    }

    public async Task<DepartmentResponse> UpdateAsync(Guid id, UpdateDepartmentRequest request, CancellationToken ct = default)
    {
        await updateValidator.ValidateAndThrowAsync(request, ct);

        var department = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"ไม่พบแผนก id '{id}'");

        // กันแผนกตั้งตัวเองเป็นแผนกแม่ (ลูปทันที)
        // หมายเหตุ: ยังไม่ทำ cycle detection แบบเต็ม (ลูปข้ามชั้น เช่น A→B→A) — เป็น logic พิเศษไว้ทำเพิ่มภายหลัง
        if (request.ParentDepartmentId == id)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure(nameof(request.ParentDepartmentId), "แผนกไม่สามารถเป็นแผนกแม่ของตัวเองได้")
            });
        }

        department.UpdateDetails(
            name: request.Name,
            nameEn: request.NameEn,
            managerId: request.ManagerId,
            parentDepartmentId: request.ParentDepartmentId);

        await repository.SaveChangesAsync(ct);
        return department.ToResponse();
    }

    public async Task ActivateAsync(Guid id, CancellationToken ct = default)
    {
        var department = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"ไม่พบแผนก id '{id}'");

        department.Activate();
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeactivateAsync(Guid id, CancellationToken ct = default)
    {
        var department = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"ไม่พบแผนก id '{id}'");

        department.Deactivate();
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var department = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"ไม่พบแผนก id '{id}'");

        repository.Remove(department);
        await repository.SaveChangesAsync(ct);
    }
}
