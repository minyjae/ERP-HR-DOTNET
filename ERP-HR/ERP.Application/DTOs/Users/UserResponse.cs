using ERP.Domain.Enums;

namespace ERP.Application.DTOs;

/// <summary>
/// ข้อมูลขาออกของบัญชีผู้ใช้
/// ⚠️ ไม่มี HashedPassword เด็ดขาด — ห้าม serialize รหัสผ่าน (แม้จะ hash แล้ว) ออกไปทาง API
/// </summary>
public record UserResponse
{
    public Guid Id { get; init; }
    public Guid EmployeeId { get; init; }
    public string Email { get; init; } = string.Empty;
    public UserRole Role { get; init; }
    public bool IsActive { get; init; }
    public DateTime? LastLoginAt { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
