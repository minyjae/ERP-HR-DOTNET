namespace ERP.Application.DTOs;

/// <summary>ข้อมูลขาเข้าตอน "สร้าง" วันหยุด (Year คำนวณจาก Date ในระบบ ไม่ต้องส่งมา)</summary>
public record CreateHolidayRequest
{
    public required string Name { get; init; }
    public required DateOnly Date { get; init; }
    public bool IsRecurring { get; init; }
}
