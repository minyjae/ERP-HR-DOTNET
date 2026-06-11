namespace ERP.Domain.Entities;

/// <summary>
/// LeaveType (ประเภทการลา เช่น ลาป่วย/ลากิจ/ลาพักร้อน) เป็น "rich domain model"
/// ทุกฟิลด์เป็น private set — สร้าง/แก้ไขต้องผ่าน factory และ method เท่านั้น
/// </summary>
public class LeaveType
{
    public Guid Id { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public bool IsPaid { get; private set; }
    public bool RequiresDocument { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // EF Core ต้องการ constructor แบบไม่มี parameter (เข้าถึงผ่าน reflection ได้แม้เป็น private)
    private LeaveType() { }

    public static LeaveType Create(
        string code,
        string name,
        bool isPaid = true,
        bool requiresDocument = false)
    {
        var now = DateTime.UtcNow;

        return new LeaveType
        {
            Id = Guid.NewGuid(),
            Code = code,
            Name = name,
            IsPaid = isPaid,
            RequiresDocument = requiresDocument,
            IsActive = true,
            CreatedAt = now,
            UpdatedAt = now
        };
    }

    /// <summary>แก้ไขข้อมูลประเภทลา (ไม่แตะ Code เพราะถือเป็นรหัสประจำตัวที่ไม่ควรเปลี่ยน)</summary>
    public void UpdateDetails(string name, bool isPaid, bool requiresDocument)
    {
        Name = name;
        IsPaid = isPaid;
        RequiresDocument = requiresDocument;
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
