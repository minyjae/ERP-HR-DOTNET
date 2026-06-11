using ERP.Domain.Enums;

namespace ERP.Domain.Entities;

/// <summary>
/// User (บัญชีผู้ใช้ระบบ) เป็น "rich domain model" ตามแนวทางใน CLAUDE.md
/// ทุกฟิลด์เป็น private set — ค่าที่มี invariant (รหัสผ่าน, สิทธิ์, สถานะ) เปลี่ยนผ่าน method เท่านั้น
/// หมายเหตุ: domain รับเฉพาะรหัสผ่านที่ "hash แล้ว" — การ hash เป็นหน้าที่ของ infrastructure (IPasswordHasher)
/// </summary>
public class User
{
    public Guid Id { get; private set; }
    public Guid EmployeeId { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string HashedPassword { get; private set; } = string.Empty;
    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime? LastLoginAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // EF Core ต้องการ constructor แบบไม่มี parameter (เข้าถึงผ่าน reflection ได้แม้เป็น private)
    private User() { }

    public static User Create(
        Guid employeeId,
        string email,
        string hashedPassword,
        UserRole role)
    {
        var now = DateTime.UtcNow;

        return new User
        {
            Id = Guid.NewGuid(),
            EmployeeId = employeeId,   // อ้าง Employee ที่มีอยู่จริง — ไม่สุ่มใหม่
            Email = email,
            HashedPassword = hashedPassword,
            Role = role,
            IsActive = true,
            LastLoginAt = null,
            CreatedAt = now,
            UpdatedAt = now
        };
    }

    /// <summary>เปลี่ยนอีเมลล็อกอิน (caller ควร validate รูปแบบก่อน)</summary>
    public void ChangeEmail(string email)
    {
        Email = email;
        Touch();
    }

    /// <summary>ตั้งรหัสผ่านใหม่ — รับค่าที่ hash มาแล้วเท่านั้น ห้ามส่ง plain text เข้ามา</summary>
    public void ChangePassword(string hashedPassword)
    {
        HashedPassword = hashedPassword;
        Touch();
    }

    /// <summary>เปลี่ยนสิทธิ์ในระบบ (เลื่อน/ลดบทบาท)</summary>
    public void ChangeRole(UserRole role)
    {
        Role = role;
        Touch();
    }

    /// <summary>บันทึกเวลาล็อกอินสำเร็จล่าสุด</summary>
    public void RecordLogin()
    {
        LastLoginAt = DateTime.UtcNow;
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
