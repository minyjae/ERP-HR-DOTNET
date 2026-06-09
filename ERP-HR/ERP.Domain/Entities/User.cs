using ERP.Domain.Enums;

namespace ERP.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public bool IsActive { get; set; }
    public DateTime? LastLoginAt { get; set; }
}