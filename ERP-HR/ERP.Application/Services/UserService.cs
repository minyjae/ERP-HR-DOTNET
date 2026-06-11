using ERP.Application.Common.Exceptions;
using ERP.Application.DTOs;
using ERP.Application.Interfaces;
using ERP.Application.Mappings;
using ERP.Domain.Entities;
using ERP.Domain.Enums;
using FluentValidation;

namespace ERP.Application.Services;

/// <summary>
/// business logic ฝั่ง User: validate → ตรวจกฎ (email/บัญชีซ้ำ) → hash รหัสผ่าน → เรียก domain → repo บันทึก
/// การ hash ทำผ่าน IPasswordHasher — service ไม่รู้จักอัลกอริทึม และ domain เก็บเฉพาะค่าที่ hash แล้ว
/// </summary>
public class UserService(
    IUserRepository repository,
    IPasswordHasher passwordHasher,
    IValidator<CreateUserRequest> createValidator,
    IValidator<UpdateUserRequest> updateValidator,
    IValidator<ChangePasswordRequest> changePasswordValidator) : IUserService
{
    public async Task<List<UserResponse>> GetAllAsync(CancellationToken ct = default)
    {
        var users = await repository.GetAllAsync(ct);
        return users.Select(u => u.ToResponse()).ToList();
    }

    public async Task<List<UserResponse>> GetByRoleAsync(UserRole role, CancellationToken ct = default)
    {
        var users = await repository.GetByRoleAsync(role, ct);
        return users.Select(u => u.ToResponse()).ToList();
    }

    public async Task<UserResponse?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var user = await repository.GetByIdAsync(id, ct);
        return user?.ToResponse();
    }

    public async Task<UserResponse> CreateAsync(CreateUserRequest request, CancellationToken ct = default)
    {
        // 1) ตรวจรูปแบบข้อมูล (โยน ValidationException → 400)
        await createValidator.ValidateAndThrowAsync(request, ct);

        // 2) กฎทางธุรกิจที่ต้องดู database
        if (await repository.ExistsByEmailAsync(request.Email, ct))
            throw new ConflictException($"อีเมล '{request.Email}' ถูกใช้งานแล้ว");

        if (await repository.ExistsByEmployeeIdAsync(request.EmployeeId, ct))
            throw new ConflictException("พนักงานคนนี้มีบัญชีผู้ใช้อยู่แล้ว");

        // 3) hash รหัสผ่าน plain → เก็บเฉพาะค่าที่ hash แล้ว
        var hashedPassword = passwordHasher.Hash(request.Password);

        // 4) สร้าง entity ผ่าน factory ของ domain
        var user = User.Create(
            employeeId: request.EmployeeId,
            email: request.Email,
            hashedPassword: hashedPassword,
            role: request.Role);

        // 5) บันทึก
        await repository.AddAsync(user, ct);
        await repository.SaveChangesAsync(ct);

        return user.ToResponse();
    }

    public async Task<UserResponse> UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken ct = default)
    {
        await updateValidator.ValidateAndThrowAsync(request, ct);

        var user = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"ไม่พบผู้ใช้ id '{id}'");

        // ถ้าเปลี่ยนอีเมล ต้องไม่ชนกับคนอื่น
        if (request.Email != user.Email && await repository.ExistsByEmailAsync(request.Email, ct))
            throw new ConflictException($"อีเมล '{request.Email}' ถูกใช้งานแล้ว");

        user.ChangeEmail(request.Email);
        user.ChangeRole(request.Role);

        await repository.SaveChangesAsync(ct);
        return user.ToResponse();
    }

    public async Task ChangePasswordAsync(Guid id, ChangePasswordRequest request, CancellationToken ct = default)
    {
        await changePasswordValidator.ValidateAndThrowAsync(request, ct);

        var user = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"ไม่พบผู้ใช้ id '{id}'");

        // ตรวจรหัสผ่านปัจจุบันก่อนอนุญาตให้เปลี่ยน
        if (!passwordHasher.Verify(request.CurrentPassword, user.HashedPassword))
            throw new ValidationException("รหัสผ่านปัจจุบันไม่ถูกต้อง");

        user.ChangePassword(passwordHasher.Hash(request.NewPassword));
        await repository.SaveChangesAsync(ct);
    }

    public async Task ActivateAsync(Guid id, CancellationToken ct = default)
    {
        var user = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"ไม่พบผู้ใช้ id '{id}'");

        user.Activate();
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeactivateAsync(Guid id, CancellationToken ct = default)
    {
        var user = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"ไม่พบผู้ใช้ id '{id}'");

        user.Deactivate();
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var user = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"ไม่พบผู้ใช้ id '{id}'");

        repository.Remove(user);
        await repository.SaveChangesAsync(ct);
    }
}
