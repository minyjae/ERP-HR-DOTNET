namespace ERP.Application.DTOs;

/// <summary>ข้อมูลขาเข้าตอน "สร้าง" นโยบายการลา</summary>
public record CreateLeavePolicyRequest
{
    public required Guid LeaveTypeId { get; init; }
    public required int EntitledDays { get; init; }
    public int MaxCarryOverDays { get; init; }
    public int MinServiceMonths { get; init; }
    public int AdvanceNoticeDays { get; init; }
}
