using ERP.Domain.Enums;

namespace ERP.Application.DTOs;

/// <summary>
/// ข้อมูลขาเข้าตอน "แก้ไข" ตำแหน่ง
/// ไม่มี Code เพราะถือเป็นรหัสประจำตัวที่ไม่ควรแก้ผ่าน endpoint นี้ (ตรงกับ Position.UpdateDetails)
/// </summary>
public record UpdatePositionRequest
{
    public required string Name { get; init; }
    public required PositionLevel Level { get; init; }
    public string? NameEn { get; init; }
    public Guid? DepartmentId { get; init; }
}
