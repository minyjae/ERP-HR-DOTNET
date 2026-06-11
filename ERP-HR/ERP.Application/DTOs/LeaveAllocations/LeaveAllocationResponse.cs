namespace ERP.Application.DTOs;

/// <summary>ข้อมูลขาออกของโควต้าวันลา (มี RemainingDays คำนวณให้พร้อมใช้)</summary>
public record LeaveAllocationResponse
{
    public Guid Id { get; init; }
    public Guid EmployeeId { get; init; }
    public Guid LeaveTypeId { get; init; }
    public int Year { get; init; }
    public int TotalDays { get; init; }
    public int UsedDays { get; init; }
    public int CarryOverDays { get; init; }
    public int RemainingDays { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
