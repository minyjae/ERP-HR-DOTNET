namespace ERP.Application.DTOs;

/// <summary>
/// ข้อมูลขาเข้าตอน "แก้ไข" นโยบายการลา
/// ไม่มี LeaveTypeId เพราะ policy ผูกกับประเภทลาเดิม (ตรงกับ LeavePolicy.UpdateDetails)
/// </summary>
public record UpdateLeavePolicyRequest
{
    public required int EntitledDays { get; init; }
    public int MaxCarryOverDays { get; init; }
    public int MinServiceMonths { get; init; }
    public int AdvanceNoticeDays { get; init; }
}
