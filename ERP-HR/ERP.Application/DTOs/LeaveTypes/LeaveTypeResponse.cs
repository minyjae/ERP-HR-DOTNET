namespace ERP.Application.DTOs;

/// <summary>ข้อมูลขาออกของประเภทการลา</summary>
public record LeaveTypeResponse
{
    public Guid Id { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public bool IsPaid { get; init; }
    public bool RequiresDocument { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
