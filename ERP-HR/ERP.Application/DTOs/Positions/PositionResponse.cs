using ERP.Domain.Enums;

namespace ERP.Application.DTOs;

/// <summary>ข้อมูลขาออกของตำแหน่งที่ส่งกลับให้ client (แยกจาก entity เพื่อไม่ผูก API contract กับ domain)</summary>
public record PositionResponse
{
    public Guid Id { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? NameEn { get; init; }
    public PositionLevel Level { get; init; }
    public Guid? DepartmentId { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
