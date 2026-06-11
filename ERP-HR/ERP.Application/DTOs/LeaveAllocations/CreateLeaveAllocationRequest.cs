namespace ERP.Application.DTOs;

/// <summary>ข้อมูลขาเข้าตอน "สร้าง" โควต้าวันลาให้พนักงาน (UsedDays เริ่มที่ 0 เสมอ)</summary>
public record CreateLeaveAllocationRequest
{
    public required Guid EmployeeId { get; init; }
    public required Guid LeaveTypeId { get; init; }
    public required int Year { get; init; }
    public required int TotalDays { get; init; }
    public int CarryOverDays { get; init; }
}
