namespace ERP.Domain.Entities;

/// <summary>
/// Department เป็น "rich domain model" (ย้ายมาจากแบบ anemic เดิม ตามแนวทางใน CLAUDE.md)
/// ทุกฟิลด์เป็น private set — สร้าง/แก้ไขต้องผ่าน factory และ method เท่านั้น
/// เพื่อบังคับ invariant และคุม UpdatedAt ให้ถูกต้องเสมอ
/// </summary>
public class Department
{
    public Guid Id { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? NameEn { get; private set; }
    public Guid? ManagerId { get; private set; }
    public Guid? ParentDepartmentId { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // EF Core ต้องการ constructor แบบไม่มี parameter (เข้าถึงผ่าน reflection ได้แม้เป็น private)
    private Department() { }

    public static Department Create(
        string code,
        string name,
        string? nameEn = null,
        Guid? managerId = null,
        Guid? parentDepartmentId = null)
    {
        var now = DateTime.UtcNow;

        return new Department
        {
            Id = Guid.NewGuid(),
            Code = code,
            Name = name,
            NameEn = nameEn,
            ManagerId = managerId,
            ParentDepartmentId = parentDepartmentId,
            IsActive = true,
            CreatedAt = now,
            UpdatedAt = now
        };
    }

    /// <summary>แก้ไขข้อมูลแผนก (ไม่แตะ Code เพราะถือเป็นรหัสประจำตัวที่ไม่ควรเปลี่ยน — เหมือน EmployeeCode)</summary>
    public void UpdateDetails(
        string name,
        string? nameEn = null,
        Guid? managerId = null,
        Guid? parentDepartmentId = null)
    {
        Name = name;
        NameEn = nameEn;
        ManagerId = managerId;
        ParentDepartmentId = parentDepartmentId;
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
