using ERP.Domain.Enums;

namespace ERP.Application.DTOs;

/// <summary>
/// ข้อมูลขาเข้าตอน "สร้าง" บัญชีผู้ใช้
/// รับ Password แบบ plain text แล้ว service จะ hash ก่อนเก็บ (domain เก็บเฉพาะค่าที่ hash แล้ว)
/// </summary>
public record CreateUserRequest
{
    public required Guid EmployeeId { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required UserRole Role { get; init; }
}
