namespace ERP.Application.DTOs;

/// <summary>ข้อมูลขาเข้าตอนปฏิเสธใบลา</summary>
public record RejectLeaveRequestRequest
{
    public required Guid ApproverId { get; init; }
    public required string RejectReason { get; init; }
}
