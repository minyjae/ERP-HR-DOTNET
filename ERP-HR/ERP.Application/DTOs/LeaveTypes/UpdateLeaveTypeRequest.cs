namespace ERP.Application.DTOs;

/// <summary>
/// ข้อมูลขาเข้าตอน "แก้ไข" ประเภทการลา
/// ไม่มี Code เพราะถือเป็นรหัสประจำตัวที่ไม่ควรแก้ผ่าน endpoint นี้ (ตรงกับ LeaveType.UpdateDetails)
/// </summary>
public record UpdateLeaveTypeRequest
{
    public required string Name { get; init; }
    public bool IsPaid { get; init; } = true;
    public bool RequiresDocument { get; init; }
}
