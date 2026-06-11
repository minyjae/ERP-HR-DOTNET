namespace ERP.Domain.Entities;

/// <summary>
/// LeavePolicy = นโยบาย/กฎเกณฑ์ของประเภทลาแต่ละชนิด (สิทธิ์ต่อปี, ยกยอด, อายุงานขั้นต่ำ, แจ้งล่วงหน้า)
/// 1 LeaveType มี policy ที่ active ได้ทีละ 1 (บังคับใน service + partial unique index)
/// rich domain — สร้าง/แก้ไขผ่าน factory + method เท่านั้น
/// </summary>
public class LeavePolicy
{
    public Guid Id { get; private set; }
    public Guid LeaveTypeId { get; private set; }
    public int EntitledDays { get; private set; }
    public int MaxCarryOverDays { get; private set; }
    public int MinServiceMonths { get; private set; }
    public int AdvanceNoticeDays { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // EF Core ต้องการ constructor แบบไม่มี parameter (เข้าถึงผ่าน reflection ได้แม้เป็น private)
    private LeavePolicy() { }

    public static LeavePolicy Create(
        Guid leaveTypeId,
        int entitledDays,
        int maxCarryOverDays,
        int minServiceMonths,
        int advanceNoticeDays)
    {
        var now = DateTime.UtcNow;

        return new LeavePolicy
        {
            Id = Guid.NewGuid(),
            LeaveTypeId = leaveTypeId,
            EntitledDays = entitledDays,
            MaxCarryOverDays = maxCarryOverDays,
            MinServiceMonths = minServiceMonths,
            AdvanceNoticeDays = advanceNoticeDays,
            IsActive = true,
            CreatedAt = now,
            UpdatedAt = now
        };
    }

    /// <summary>แก้ไขกฎเกณฑ์ (ไม่แตะ LeaveTypeId เพราะ policy ผูกกับประเภทลาเดิม)</summary>
    public void UpdateDetails(
        int entitledDays,
        int maxCarryOverDays,
        int minServiceMonths,
        int advanceNoticeDays)
    {
        EntitledDays = entitledDays;
        MaxCarryOverDays = maxCarryOverDays;
        MinServiceMonths = minServiceMonths;
        AdvanceNoticeDays = advanceNoticeDays;
        Touch();
    }

    public void Activate()
    {
        IsActive = true;
        Touch();
    }

    public void Deactivate()
    {
        IsActive = false;
        Touch();
    }

    private void Touch() => UpdatedAt = DateTime.UtcNow;
}
