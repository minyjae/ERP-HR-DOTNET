namespace ERP.Application.DTOs;

/// <summary>
/// ข้อมูลขาเข้าตอน "แก้ไข" สาขา
/// ไม่มี Code เพราะถือเป็นรหัสประจำตัวที่ไม่ควรแก้ผ่าน endpoint นี้ (ตรงกับ Branch.UpdateDetails)
/// </summary>
public record UpdateBranchRequest
{
    public required string Name { get; init; }
    public string? Address { get; init; }
    public string? Phone { get; init; }
}
