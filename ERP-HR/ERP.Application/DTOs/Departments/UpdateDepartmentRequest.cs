namespace ERP.Application.DTOs;

/// <summary>
/// ข้อมูลขาเข้าตอน "แก้ไข" แผนก
/// ไม่มี Code เพราะถือเป็นรหัสประจำตัวที่ไม่ควรแก้ผ่าน endpoint นี้ (ตรงกับ Department.UpdateDetails)
/// </summary>
public record UpdateDepartmentRequest
{
    public required string Name { get; init; }
    public string? NameEn { get; init; }
    public Guid? ManagerId { get; init; }
    public Guid? ParentDepartmentId { get; init; }
}
