namespace ERP.Domain.Entities;

/// <summary>
/// Holiday (วันหยุดประจำปีของบริษัท) — ใช้ตอนคำนวณจำนวนวันลาจริงในโมดูล Leave
/// rich domain — สร้าง/แก้ไขผ่าน factory + method เท่านั้น
/// Year ผูกกับ Date เสมอ (คำนวณให้อัตโนมัติ) เพื่อกันข้อมูลขัดแย้งกันเอง
/// </summary>
public class Holiday
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public DateOnly Date { get; private set; }
    public int Year { get; private set; }
    public bool IsRecurring { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // EF Core ต้องการ constructor แบบไม่มี parameter (เข้าถึงผ่าน reflection ได้แม้เป็น private)
    private Holiday() { }

    public static Holiday Create(string name, DateOnly date, bool isRecurring = false)
    {
        var now = DateTime.UtcNow;

        return new Holiday
        {
            Id = Guid.NewGuid(),
            Name = name,
            Date = date,
            Year = date.Year,        // ผูกกับ Date เสมอ
            IsRecurring = isRecurring,
            CreatedAt = now,
            UpdatedAt = now
        };
    }

    public void UpdateDetails(string name, DateOnly date, bool isRecurring)
    {
        Name = name;
        Date = date;
        Year = date.Year;
        IsRecurring = isRecurring;
        Touch();
    }

    private void Touch() => UpdatedAt = DateTime.UtcNow;
}
