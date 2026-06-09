using ERP.Domain.Enums;

namespace ERP.Domain.Entities;

public class LeaveRequest
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public Guid LeaveTypeId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public decimal TotalDays { get; set; }
    public string? Reason { get; set; }
    public string? AttachmentUrl { get; set; }
    public LeaveRequestStatus Status { get; set; }
    public DateTime RequestedAt { get; set; }
    public Guid? ApprovedById { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string? RejectReason { get; set; }
    public DateTime? CancelledAt { get; set; }
}