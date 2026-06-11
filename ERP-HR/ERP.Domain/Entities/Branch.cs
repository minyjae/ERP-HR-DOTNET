namespace ERP.Domain.Entities;

/// <summary>
/// Branch (สาขา) เป็น "rich domain model" (ย้ายมาจากแบบ anemic เดิม ตามแนวทางใน CLAUDE.md)
/// ทุกฟิลด์เป็น private set — สร้าง/แก้ไขต้องผ่าน factory และ method เท่านั้น
/// </summary>
public class Branch
{
    public Guid Id { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? Address { get; private set; }
    public string? Phone { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // EF Core ต้องการ constructor แบบไม่มี parameter (เข้าถึงผ่าน reflection ได้แม้เป็น private)
    private Branch() { }

    public static Branch Create(
        string code,
        string name,
        string? address = null,
        string? phone = null)
    {
        var now = DateTime.UtcNow;

        return new Branch
        {
            Id = Guid.NewGuid(),
            Code = code,
            Name = name,
            Address = address,
            Phone = phone,
            IsActive = true,
            CreatedAt = now,
            UpdatedAt = now
        };
    }

    /// <summary>แก้ไขข้อมูลสาขา (ไม่แตะ Code เพราะถือเป็นรหัสประจำตัวที่ไม่ควรเปลี่ยน)</summary>
    public void UpdateDetails(
        string name,
        string? address = null,
        string? phone = null)
    {
        Name = name;
        Address = address;
        Phone = phone;
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
