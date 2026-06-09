namespace ERP.Domain.Entities;

public class LeaveType
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsPaid { get; set; }
    public bool RequiresDocument { get; set; }
    public bool IsActive { get; set; }
}