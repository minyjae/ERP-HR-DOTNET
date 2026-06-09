using ERP.Domain.Enums;

namespace ERP.Application.DTOs;

/// <summary>ข้อมูลขาเข้าตอน "สร้าง" พนักงานใหม่ (มาจาก request body)</summary>
public record CreateEmployeeRequest
{
    public required string EmployeeCode { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public string? FirstNameEn { get; init; }
    public string? LastNameEn { get; init; }
    public required DateOnly DateOfBirth { get; init; }
    public required Gender Gender { get; init; }
    public required string NationalId { get; init; }
    public string? PhoneNumber { get; init; }
    public required string Email { get; init; }
    public string? Address { get; init; }
    public string? ProfileImageUrl { get; init; }
    public required DateOnly HireDate { get; init; }
    public required Guid DepartmentId { get; init; }
    public required Guid PositionId { get; init; }
    public required Guid BranchId { get; init; }
    public Guid? ManagerId { get; init; }
}
