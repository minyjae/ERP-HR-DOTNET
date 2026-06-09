using ERP.Application.Common.Exceptions;
using ERP.Application.DTOs;
using ERP.Application.Interfaces;
using ERP.Application.Mappings;
using ERP.Domain.Entities;
using ERP.Domain.Enums;
using FluentValidation;

namespace ERP.Application.Services;

/// <summary>
/// หัวใจของ business logic: validate → ตรวจกฎ (ซ้ำ/มีอยู่จริง) → เรียก domain → สั่ง repo บันทึก
/// service ไม่รู้จัก HTTP และไม่รู้จัก EF Core — รับ/ส่งเป็น DTO กับ entity เท่านั้น
/// </summary>
public class EmployeeService(
    IEmployeeRepository repository,
    IValidator<CreateEmployeeRequest> createValidator,
    IValidator<UpdateEmployeeRequest> updateValidator) : IEmployeeService
{
    public async Task<List<EmployeeResponse>> GetAllAsync(CancellationToken ct = default)
    {
        var employees = await repository.GetAllAsync(ct);
        return employees.Select(e => e.ToResponse()).ToList();
    }

    public async Task<EmployeeResponse?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var employee = await repository.GetByIdAsync(id, ct);
        return employee?.ToResponse();
    }

    public async Task<EmployeeResponse> CreateAsync(CreateEmployeeRequest request, CancellationToken ct = default)
    {
        // 1) ตรวจรูปแบบข้อมูลด้วย FluentValidation (โยน ValidationException ถ้าไม่ผ่าน → 400)
        await createValidator.ValidateAndThrowAsync(request, ct);

        // 2) ตรวจกฎทางธุรกิจที่ต้องดู database (ค่าซ้ำ)
        if (await repository.ExistsByEmployeeCodeAsync(request.EmployeeCode, ct))
            throw new ConflictException($"รหัสพนักงาน '{request.EmployeeCode}' ถูกใช้งานแล้ว");

        if (await repository.ExistsByEmailAsync(request.Email, ct))
            throw new ConflictException($"อีเมล '{request.Email}' ถูกใช้งานแล้ว");

        if (await repository.ExistsByNationalIdAsync(request.NationalId, ct))
            throw new ConflictException($"เลขบัตรประชาชน '{request.NationalId}' ถูกใช้งานแล้ว");

        // 3) สร้าง entity ผ่าน factory ของ domain
        var employee = Employee.Create(
            employeeCode: request.EmployeeCode,
            firstName: request.FirstName,
            lastName: request.LastName,
            dateOfBirth: request.DateOfBirth,
            gender: request.Gender,
            nationalId: request.NationalId,
            email: request.Email,
            hireDate: request.HireDate,
            departmentId: request.DepartmentId,
            positionId: request.PositionId,
            branchId: request.BranchId,
            firstNameEn: request.FirstNameEn,
            lastNameEn: request.LastNameEn,
            phoneNumber: request.PhoneNumber,
            address: request.Address,
            profileImageUrl: request.ProfileImageUrl,
            managerId: request.ManagerId);

        // 4) บันทึก
        await repository.AddAsync(employee, ct);
        await repository.SaveChangesAsync(ct);

        return employee.ToResponse();
    }

    public async Task<EmployeeResponse> UpdateAsync(Guid id, UpdateEmployeeRequest request, CancellationToken ct = default)
    {
        await updateValidator.ValidateAndThrowAsync(request, ct);

        var employee = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"ไม่พบพนักงาน id '{id}'");

        // กันอีเมลซ้ำกับ "คนอื่น" (ของตัวเองไม่นับ)
        if (!string.Equals(employee.Email, request.Email, StringComparison.OrdinalIgnoreCase)
            && await repository.ExistsByEmailAsync(request.Email, ct))
        {
            throw new ConflictException($"อีเมล '{request.Email}' ถูกใช้งานแล้ว");
        }

        employee.UpdateDetails(
            firstName: request.FirstName,
            lastName: request.LastName,
            gender: request.Gender,
            email: request.Email,
            departmentId: request.DepartmentId,
            positionId: request.PositionId,
            branchId: request.BranchId,
            firstNameEn: request.FirstNameEn,
            lastNameEn: request.LastNameEn,
            phoneNumber: request.PhoneNumber,
            address: request.Address,
            profileImageUrl: request.ProfileImageUrl,
            managerId: request.ManagerId);

        await repository.SaveChangesAsync(ct);
        return employee.ToResponse();
    }

    public async Task ResignAsync(Guid id, CancellationToken ct = default)
    {
        var employee = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"ไม่พบพนักงาน id '{id}'");

        employee.ChangeStatus(EmployeeStatus.Resigned);
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var employee = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"ไม่พบพนักงาน id '{id}'");

        repository.Remove(employee);
        await repository.SaveChangesAsync(ct);
    }
}
