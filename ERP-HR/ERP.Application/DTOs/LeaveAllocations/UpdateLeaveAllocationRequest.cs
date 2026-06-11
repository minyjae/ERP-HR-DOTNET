namespace ERP.Application.DTOs;

/// <summary>
/// ปรับโควต้า/ยอดยกมา (ไม่แตะ UsedDays — การใช้วันลาเปลี่ยนผ่านการอนุมัติใบลาเท่านั้น)
/// ตรงกับ LeaveAllocation.Adjust
/// </summary>
public record UpdateLeaveAllocationRequest
{
    public required int TotalDays { get; init; }
    public int CarryOverDays { get; init; }
}
