namespace ERP.Domain.Entities;

/// <summary>
/// LeaveAllocation = โควต้าวันลาของพนักงาน 1 คน ต่อ 1 ประเภทลา ต่อ 1 ปี
/// rich domain — การหัก/คืนโควต้าต้องผ่าน Consume/Release เพื่อกัน UsedDays ติดลบหรือเกินสิทธิ์
/// </summary>
public class LeaveAllocation
{
    public Guid Id { get; private set; }
    public Guid EmployeeId { get; private set; }
    public Guid LeaveTypeId { get; private set; }
    public int Year { get; private set; }
    public int TotalDays { get; private set; }
    public int UsedDays { get; private set; }
    public int CarryOverDays { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    /// <summary>วันลาคงเหลือ = สิทธิ์ + ยกยอด − ใช้ไป (computed, ไม่เก็บลง DB → [Ignore])</summary>
    public int RemainingDays => TotalDays + CarryOverDays - UsedDays;

    // EF Core ต้องการ constructor แบบไม่มี parameter (เข้าถึงผ่าน reflection ได้แม้เป็น private)
    private LeaveAllocation() { }

    public static LeaveAllocation Create(
        Guid employeeId,
        Guid leaveTypeId,
        int year,
        int totalDays,
        int carryOverDays = 0)
    {
        var now = DateTime.UtcNow;

        return new LeaveAllocation
        {
            Id = Guid.NewGuid(),
            EmployeeId = employeeId,
            LeaveTypeId = leaveTypeId,
            Year = year,
            TotalDays = totalDays,
            CarryOverDays = carryOverDays,
            UsedDays = 0,
            CreatedAt = now,
            UpdatedAt = now
        };
    }

    /// <summary>หักโควต้า (เรียกตอนใบลาได้รับอนุมัติ) — กันใช้เกินคงเหลือ</summary>
    public void Consume(int days)
    {
        if (days <= 0)
            throw new InvalidOperationException("จำนวนวันที่หักต้องมากกว่า 0");
        if (days > RemainingDays)
            throw new InvalidOperationException($"วันลาคงเหลือไม่พอ (เหลือ {RemainingDays} ขอ {days})");

        UsedDays += days;
        Touch();
    }

    /// <summary>คืนโควต้า (เรียกตอนยกเลิกใบลาที่อนุมัติแล้ว) — กัน UsedDays ติดลบ</summary>
    public void Release(int days)
    {
        if (days <= 0)
            throw new InvalidOperationException("จำนวนวันที่คืนต้องมากกว่า 0");
        if (days > UsedDays)
            throw new InvalidOperationException($"คืนวันลาเกินที่ใช้ไป (ใช้ไป {UsedDays} คืน {days})");

        UsedDays -= days;
        Touch();
    }

    /// <summary>ปรับโควต้า/ยอดยกมา (เช่น HR แก้ไขสิทธิ์) — ไม่แตะ UsedDays</summary>
    public void Adjust(int totalDays, int carryOverDays)
    {
        if (totalDays < 0 || carryOverDays < 0)
            throw new InvalidOperationException("จำนวนวันต้องไม่ติดลบ");

        TotalDays = totalDays;
        CarryOverDays = carryOverDays;
        Touch();
    }

    private void Touch() => UpdatedAt = DateTime.UtcNow;
}
