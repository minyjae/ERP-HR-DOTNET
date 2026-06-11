namespace ERP.Application.DTOs;

/// <summary>ข้อมูลขาเข้าตอน "สร้าง" แผนกใหม่ (มาจาก request body)</summary>
public record CreateDepartmentRequest
{
    public required string Code { get; init; }
    public required string Name { get; init; }
    public string? NameEn { get; init; }
    public Guid? ManagerId { get; init; }
    public Guid? ParentDepartmentId { get; init; }
}
