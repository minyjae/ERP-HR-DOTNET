namespace ERP.Application.DTOs;

/// <summary>ข้อมูลขาออกของ record ประวัติตำแหน่ง</summary>
public record EmployeePositionResponse
{
    public Guid Id { get; init; }
    public Guid EmployeeId { get; init; }
    public Guid PositionId { get; init; }
    public Guid DepartmentId { get; init; }
    public DateOnly StartDate { get; init; }
    public DateOnly? EndDate { get; init; }
    public decimal Salary { get; init; }
    public string? Remark { get; init; }
    public bool IsCurrent { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
