namespace ERP.Application.DTOs;

/// <summary>
/// ข้อมูลขาเข้าตอน "ยื่น" ใบลา
/// ไม่มี TotalDays — ระบบคำนวณเองจากช่วงวันที่ (หักเสาร์-อาทิตย์ + วันหยุด)
/// </summary>
public record CreateLeaveRequestRequest
{
    public required Guid EmployeeId { get; init; }
    public required Guid LeaveTypeId { get; init; }
    public required DateOnly StartDate { get; init; }
    public required DateOnly EndDate { get; init; }
    public string? Reason { get; init; }
    public string? AttachmentUrl { get; init; }
}
