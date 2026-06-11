using ERP.Domain.Enums;

namespace ERP.Domain.Entities;

/// <summary>
/// Position (ตำแหน่งงาน) เป็น "rich domain model" (ย้ายมาจากแบบ anemic เดิม ตามแนวทางใน CLAUDE.md)
/// ทุกฟิลด์เป็น private set — สร้าง/แก้ไขต้องผ่าน factory และ method เท่านั้น
/// เพื่อบังคับ invariant และคุม UpdatedAt ให้ถูกต้องเสมอ
/// </summary>
public class Position
{
    public Guid Id { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? NameEn { get; private set; }
    public PositionLevel Level { get; private set; }
    public Guid? DepartmentId { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // EF Core ต้องการ constructor แบบไม่มี parameter (เข้าถึงผ่าน reflection ได้แม้เป็น private)
    private Position() { }

    public static Position Create(
        string code,
        string name,
        PositionLevel level,
        string? nameEn = null,
        Guid? departmentId = null)
    {
        var now = DateTime.UtcNow;

        return new Position
        {
            Id = Guid.NewGuid(),
            Code = code,
            Name = name,
            Level = level,
            NameEn = nameEn,
            DepartmentId = departmentId,
            IsActive = true,
            CreatedAt = now,
            UpdatedAt = now
        };
    }

    /// <summary>แก้ไขข้อมูลตำแหน่ง (ไม่แตะ Code เพราะถือเป็นรหัสประจำตัวที่ไม่ควรเปลี่ยน)</summary>
    public void UpdateDetails(
        string name,
        PositionLevel level,
        string? nameEn = null,
        Guid? departmentId = null)
    {
        Name = name;
        Level = level;
        NameEn = nameEn;
        DepartmentId = departmentId;
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
