using ERP.Domain.Enums;

namespace ERP.Domain.Entities;

/// <summary>
/// Employee เป็น "rich domain model": ฟิลด์ทุกตัวเป็น private set
/// การสร้าง/แก้ไขต้องผ่าน factory และ method เท่านั้น เพื่อบังคับ invariant
/// และคุม UpdatedAt ให้ถูกต้องเสมอ (ไม่ปล่อยให้ layer ภายนอกเซ็ตค่ามั่ว ๆ)
/// </summary>
public class Employee
{
    public Guid Id { get; private set; }
    public string EmployeeCode { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string? FirstNameEn { get; private set; }
    public string? LastNameEn { get; private set; }
    public DateOnly DateOfBirth { get; private set; }
    public Gender Gender { get; private set; }
    public string NationalId { get; private set; } = string.Empty;
    public string? PhoneNumber { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string? Address { get; private set; }
    public string? ProfileImageUrl { get; private set; }
    public DateOnly HireDate { get; private set; }
    public EmployeeStatus Status { get; private set; }
    public Guid DepartmentId { get; private set; }
    public Guid PositionId { get; private set; }
    public Guid BranchId { get; private set; }
    public Guid? ManagerId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // EF Core ต้องการ constructor แบบไม่มี parameter (ใช้ private ได้ EF เข้าถึงผ่าน reflection)
    private Employee() { }

    public static Employee Create(
        string employeeCode,
        string firstName,
        string lastName,
        DateOnly dateOfBirth,
        Gender gender,
        string nationalId,
        string email,
        DateOnly hireDate,
        Guid departmentId,
        Guid positionId,
        Guid branchId,
        string? firstNameEn = null,
        string? lastNameEn = null,
        string? phoneNumber = null,
        string? address = null,
        string? profileImageUrl = null,
        Guid? managerId = null)
    {
        var now = DateTime.UtcNow;

        return new Employee
        {
            Id = Guid.NewGuid(),
            EmployeeCode = employeeCode,
            FirstName = firstName,
            LastName = lastName,
            FirstNameEn = firstNameEn,
            LastNameEn = lastNameEn,
            DateOfBirth = dateOfBirth,
            Gender = gender,
            NationalId = nationalId,
            PhoneNumber = phoneNumber,
            Email = email,
            Address = address,
            ProfileImageUrl = profileImageUrl,
            HireDate = hireDate,
            Status = EmployeeStatus.Active,
            DepartmentId = departmentId,
            PositionId = positionId,
            BranchId = branchId,
            ManagerId = managerId,
            CreatedAt = now,
            UpdatedAt = now
        };
    }

    /// <summary>แก้ไขข้อมูลส่วนตัว/การสังกัด (ไม่แตะ EmployeeCode, NationalId, HireDate, Status)</summary>
    public void UpdateDetails(
        string firstName,
        string lastName,
        Gender gender,
        string email,
        Guid departmentId,
        Guid positionId,
        Guid branchId,
        string? firstNameEn = null,
        string? lastNameEn = null,
        string? phoneNumber = null,
        string? address = null,
        string? profileImageUrl = null,
        Guid? managerId = null)
    {
        FirstName = firstName;
        LastName = lastName;
        FirstNameEn = firstNameEn;
        LastNameEn = lastNameEn;
        Gender = gender;
        Email = email;
        PhoneNumber = phoneNumber;
        Address = address;
        ProfileImageUrl = profileImageUrl;
        DepartmentId = departmentId;
        PositionId = positionId;
        BranchId = branchId;
        ManagerId = managerId;
        Touch();
    }

    /// <summary>เปลี่ยนสถานะพนักงาน (ผ่าน method เพื่อใส่ business rule เพิ่มได้ภายหลัง)</summary>
    public void ChangeStatus(EmployeeStatus status)
    {
        Status = status;
        Touch();
    }

    private void Touch() => UpdatedAt = DateTime.UtcNow;
}
