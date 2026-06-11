using ERP.Domain.Enums;

namespace ERP.Application.DTOs;

/// <summary>
/// ข้อมูลขาเข้าตอน "แก้ไข" บัญชีผู้ใช้
/// ไม่มี EmployeeId (เปลี่ยนเจ้าของบัญชีไม่ได้) และไม่มี Password (เปลี่ยนผ่าน endpoint แยก = ChangePasswordRequest)
/// ตรงกับ User.ChangeEmail / User.ChangeRole
/// </summary>
public record UpdateUserRequest
{
    public required string Email { get; init; }
    public required UserRole Role { get; init; }
}