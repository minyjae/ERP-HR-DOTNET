using ERP.Domain.Enums;

namespace ERP.Application.DTOs;

/// <summary>ข้อมูลขาเข้าตอน "สร้าง" ตำแหน่งใหม่ (มาจาก request body)</summary>
public record CreatePositionRequest
{
    public required string Code { get; init; }
    public required string Name { get; init; }
    public required PositionLevel Level { get; init; }
    public string? NameEn { get; init; }
    public Guid? DepartmentId { get; init; }
}
