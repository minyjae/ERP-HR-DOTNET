namespace ERP.Application.DTOs;

/// <summary>ข้อมูลขาออกของนโยบายการลา</summary>
public record LeavePolicyResponse
{
    public Guid Id { get; init; }
    public Guid LeaveTypeId { get; init; }
    public int EntitledDays { get; init; }
    public int MaxCarryOverDays { get; init; }
    public int MinServiceMonths { get; init; }
    public int AdvanceNoticeDays { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
