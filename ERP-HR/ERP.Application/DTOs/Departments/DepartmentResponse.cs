namespace ERP.Application.DTOs;

/// <summary>ข้อมูลขาออกของแผนกที่ส่งกลับให้ client (แยกจาก entity เพื่อไม่ผูก API contract กับ domain)</summary>
public record DepartmentResponse
{
    public Guid Id { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? NameEn { get; init; }
    public Guid? ManagerId { get; init; }
    public Guid? ParentDepartmentId { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
