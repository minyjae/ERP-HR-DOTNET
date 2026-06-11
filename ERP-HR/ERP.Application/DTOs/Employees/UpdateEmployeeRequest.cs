using ERP.Domain.Enums;

namespace ERP.Application.DTOs;

/// <summary>
/// ข้อมูลขาเข้าตอน "แก้ไข" พนักงาน
/// สังเกตว่าไม่มี EmployeeCode / NationalId / HireDate เพราะถือเป็นข้อมูลที่ไม่ควรแก้ผ่าน endpoint นี้
/// (ตรงกับ Employee.UpdateDetails ใน domain)
/// </summary>
public record UpdateEmployeeRequest
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public string? FirstNameEn { get; init; }
    public string? LastNameEn { get; init; }
    public required Gender Gender { get; init; }
    public required string Email { get; init; }
    public string? PhoneNumber { get; init; }
    public string? Address { get; init; }
    public string? ProfileImageUrl { get; init; }
    public required Guid DepartmentId { get; init; }
    public required Guid PositionId { get; init; }
    public required Guid BranchId { get; init; }
    public Guid? ManagerId { get; init; }
}
