using ERP.Application.DTOs;
using ERP.Domain.Entities;

namespace ERP.Application.Mappings;

/// <summary>
/// แปลง User entity → DTO ด้วย extension method แบบเขียนมือ (สไตล์เดียวกับ Branch/Employee)
/// ⚠️ ไม่ map HashedPassword ออกไป — UserResponse ไม่มี field นั้นโดยตั้งใจ
/// </summary>
public static class UserMappingExtensions
{
    public static UserResponse ToResponse(this User u) => new()
    {
        Id = u.Id,
        EmployeeId = u.EmployeeId,
        Email = u.Email,
        Role = u.Role,
        IsActive = u.IsActive,
        LastLoginAt = u.LastLoginAt,
        CreatedAt = u.CreatedAt,
        UpdatedAt = u.UpdatedAt
    };
}
