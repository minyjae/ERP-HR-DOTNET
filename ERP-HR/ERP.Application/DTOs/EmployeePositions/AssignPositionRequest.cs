namespace ERP.Application.DTOs;

/// <summary>
/// มอบหมายตำแหน่งใหม่ให้พนักงาน — ระบบจะปิดตำแหน่งปัจจุบัน (ถ้ามี) ให้อัตโนมัติ
/// </summary>
public record AssignPositionRequest
{
    public required Guid EmployeeId { get; init; }
    public required Guid PositionId { get; init; }
    public required Guid DepartmentId { get; init; }
    public required DateOnly StartDate { get; init; }
    public required decimal Salary { get; init; }
    public string? Remark { get; init; }
}
