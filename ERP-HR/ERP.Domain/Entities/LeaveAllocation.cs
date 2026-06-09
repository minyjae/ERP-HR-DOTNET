namespace ERP.Domain.Entities;

public class LeaveAllocation
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public Guid LeaveTypeId { get; set; }
    public int Year { get; set; }
    public int TotalDays { get; set; }
    public int UsedDays { get; set; }
    public int CarryOverDays { get; set; }
    public int RemainingDays => TotalDays + CarryOverDays - UsedDays;
}