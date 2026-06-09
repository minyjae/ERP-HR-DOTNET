namespace ERP.Domain.Entities;

public class EmployeePosition
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public Guid PositionId { get; set; }
    public Guid DepartmentId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public decimal Salary { get; set; }
    public string? Remark { get; set; }
}