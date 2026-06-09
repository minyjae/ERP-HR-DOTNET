namespace ERP.Domain.Entities;

public class LeavePolicy
{
    public Guid Id { get; set; }
    public Guid LeaveTypeId { get; set; }
    public int EntitledDays { get; set; }
    public int MaxCarryOverDays { get; set; }
    public int MinServiceMonths { get; set; }
    public int AdvanceNoticeDays { get; set; }
    public bool IsActive { get; set; }
}