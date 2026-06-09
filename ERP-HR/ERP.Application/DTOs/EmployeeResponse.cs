using ERP.Domain.Enums;

namespace ERP.Application.DTOs;

/// <summary>
/// ข้อมูลขาออกที่ส่งกลับให้ client
/// แยกจาก entity เพื่อไม่ผูก API contract กับ domain model โดยตรง
/// (อยากเปลี่ยน entity ภายในได้โดยไม่กระทบ client)
/// </summary>
public record EmployeeResponse
{
    public Guid Id { get; init; }
    public string EmployeeCode { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string? FirstNameEn { get; init; }
    public string? LastNameEn { get; init; }
    public DateOnly DateOfBirth { get; init; }
    public Gender Gender { get; init; }
    public string NationalId { get; init; } = string.Empty;
    public string? PhoneNumber { get; init; }
    public string Email { get; init; } = string.Empty;
    public string? Address { get; init; }
    public string? ProfileImageUrl { get; init; }
    public DateOnly HireDate { get; init; }
    public EmployeeStatus Status { get; init; }
    public Guid DepartmentId { get; init; }
    public Guid PositionId { get; init; }
    public Guid BranchId { get; init; }
    public Guid? ManagerId { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
