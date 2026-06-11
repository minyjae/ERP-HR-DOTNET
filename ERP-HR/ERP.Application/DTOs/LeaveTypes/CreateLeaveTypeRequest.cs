namespace ERP.Application.DTOs;

/// <summary>ข้อมูลขาเข้าตอน "สร้าง" ประเภทการลา</summary>
public record CreateLeaveTypeRequest
{
    public required string Code { get; init; }
    public required string Name { get; init; }
    public bool IsPaid { get; init; } = true;
    public bool RequiresDocument { get; init; }
}
