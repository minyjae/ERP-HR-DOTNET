namespace ERP.Application.DTOs;

/// <summary>ข้อมูลขาเข้าตอน "แก้ไข" วันหยุด</summary>
public record UpdateHolidayRequest
{
    public required string Name { get; init; }
    public required DateOnly Date { get; init; }
    public bool IsRecurring { get; init; }
}
