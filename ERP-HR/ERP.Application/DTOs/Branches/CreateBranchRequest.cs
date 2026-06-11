namespace ERP.Application.DTOs;

/// <summary>ข้อมูลขาเข้าตอน "สร้าง" สาขาใหม่ (มาจาก request body)</summary>
public record CreateBranchRequest
{
    public required string Code { get; init; }
    public required string Name { get; init; }
    public string? Address { get; init; }
    public string? Phone { get; init; }
}
