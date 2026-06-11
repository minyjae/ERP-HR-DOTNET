using ERP.Domain.Enums;

namespace ERP.Application.DTOs;

/// <summary>ข้อมูลขาออกของใบลา</summary>
public record LeaveRequestResponse
{
    public Guid Id { get; init; }
    public Guid EmployeeId { get; init; }
    public Guid LeaveTypeId { get; init; }
    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init; }
    public decimal TotalDays { get; init; }
    public string? Reason { get; init; }
    public string? AttachmentUrl { get; init; }
    public LeaveRequestStatus Status { get; init; }
    public DateTime RequestedAt { get; init; }
    public Guid? ApprovedById { get; init; }
    public DateTime? ApprovedAt { get; init; }
    public string? RejectReason { get; init; }
    public DateTime? CancelledAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
