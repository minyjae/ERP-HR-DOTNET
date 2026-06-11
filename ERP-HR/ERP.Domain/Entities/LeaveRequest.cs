using ERP.Domain.Enums;

namespace ERP.Domain.Entities;

/// <summary>
/// LeaveRequest (ใบลา) — entity ที่มี state machine: Pending → Approved/Rejected/Cancelled
/// rich domain: เปลี่ยนสถานะผ่าน method ที่ตรวจ transition เท่านั้น
/// TotalDays ถูกคำนวณจากภายนอก (service) เพราะต้องใช้ข้อมูลวันหยุด ซึ่ง domain ไม่รู้จัก
/// </summary>
public class LeaveRequest
{
    public Guid Id { get; private set; }
    public Guid EmployeeId { get; private set; }
    public Guid LeaveTypeId { get; private set; }
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }
    public decimal TotalDays { get; private set; }
    public string? Reason { get; private set; }
    public string? AttachmentUrl { get; private set; }
    public LeaveRequestStatus Status { get; private set; }
    public DateTime RequestedAt { get; private set; }
    public Guid? ApprovedById { get; private set; }
    public DateTime? ApprovedAt { get; private set; }
    public string? RejectReason { get; private set; }
    public DateTime? CancelledAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // EF Core ต้องการ constructor แบบไม่มี parameter (เข้าถึงผ่าน reflection ได้แม้เป็น private)
    private LeaveRequest() { }

    public static LeaveRequest Create(
        Guid employeeId,
        Guid leaveTypeId,
        DateOnly startDate,
        DateOnly endDate,
        decimal totalDays,
        string? reason = null,
        string? attachmentUrl = null)
    {
        if (endDate < startDate)
            throw new InvalidOperationException("วันสิ้นสุดต้องไม่ก่อนวันเริ่ม");
        if (totalDays <= 0)
            throw new InvalidOperationException("จำนวนวันลาต้องมากกว่า 0");

        var now = DateTime.UtcNow;

        return new LeaveRequest
        {
            Id = Guid.NewGuid(),
            EmployeeId = employeeId,
            LeaveTypeId = leaveTypeId,
            StartDate = startDate,
            EndDate = endDate,
            TotalDays = totalDays,
            Reason = reason,
            AttachmentUrl = attachmentUrl,
            Status = LeaveRequestStatus.Pending,
            RequestedAt = now,
            UpdatedAt = now
        };
    }

    /// <summary>อนุมัติ (ทำได้เฉพาะ Pending)</summary>
    public void Approve(Guid approverId)
    {
        EnsurePending();
        Status = LeaveRequestStatus.Approved;
        ApprovedById = approverId;
        ApprovedAt = DateTime.UtcNow;
        Touch();
    }

    /// <summary>ปฏิเสธ (ทำได้เฉพาะ Pending)</summary>
    public void Reject(Guid approverId, string reason)
    {
        EnsurePending();
        Status = LeaveRequestStatus.Rejected;
        ApprovedById = approverId;
        ApprovedAt = DateTime.UtcNow;
        RejectReason = reason;
        Touch();
    }

    /// <summary>ยกเลิก (ทำได้ตอน Pending หรือ Approved)</summary>
    public void Cancel()
    {
        if (Status is not (LeaveRequestStatus.Pending or LeaveRequestStatus.Approved))
            throw new InvalidOperationException($"ยกเลิกใบลาสถานะ {Status} ไม่ได้");

        Status = LeaveRequestStatus.Cancelled;
        CancelledAt = DateTime.UtcNow;
        Touch();
    }

    private void EnsurePending()
    {
        if (Status != LeaveRequestStatus.Pending)
            throw new InvalidOperationException($"ดำเนินการกับใบลาสถานะ {Status} ไม่ได้ (ต้องเป็น Pending)");
    }

    private void Touch() => UpdatedAt = DateTime.UtcNow;
}
