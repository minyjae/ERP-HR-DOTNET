namespace ERP.Application.DTOs;

/// <summary>ข้อมูลขาเข้าตอนอนุมัติใบลา (ภายหลังเมื่อมีระบบ Auth จะดึง ApproverId จาก token แทน)</summary>
public record ApproveLeaveRequestRequest
{
    public required Guid ApproverId { get; init; }
}
