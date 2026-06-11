namespace ERP.Application.DTOs;

/// <summary>ข้อมูลขาออกของวันหยุด</summary>
public record HolidayResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public DateOnly Date { get; init; }
    public int Year { get; init; }
    public bool IsRecurring { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
