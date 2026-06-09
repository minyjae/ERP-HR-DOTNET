using ERP.Domain.Enums;

namespace ERP.Domain.Entities;

public class Position
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? NameEn { get; set; }
    public PositionLevel Level { get; set; }
    public Guid? DepartmentId { get; set; }
    public bool IsActive { get; set; }
}