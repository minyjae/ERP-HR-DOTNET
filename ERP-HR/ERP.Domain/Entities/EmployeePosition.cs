namespace ERP.Domain.Entities;

/// <summary>
/// EmployeePosition = ประวัติการดำรงตำแหน่งของพนักงาน (timeline)
/// 1 พนักงานมีได้หลาย record แต่ "ตำแหน่งปัจจุบัน" (EndDate == null) ได้ทีละ 1
/// rich domain — สร้าง/ปิด/แก้ไขผ่าน factory + method เท่านั้น
/// </summary>
public class EmployeePosition
{
    public Guid Id { get; private set; }
    public Guid EmployeeId { get; private set; }
    public Guid PositionId { get; private set; }
    public Guid DepartmentId { get; private set; }
    public DateOnly StartDate { get; private set; }
    public DateOnly? EndDate { get; private set; }
    public decimal Salary { get; private set; }
    public string? Remark { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    /// <summary>true = ตำแหน่งปัจจุบัน (ยังไม่ปิดช่วงเวลา)</summary>
    public bool IsCurrent => EndDate is null;

    // EF Core ต้องการ constructor แบบไม่มี parameter (เข้าถึงผ่าน reflection ได้แม้เป็น private)
    private EmployeePosition() { }

    public static EmployeePosition Create(
        Guid employeeId,
        Guid positionId,
        Guid departmentId,
        DateOnly startDate,
        decimal salary,
        string? remark = null)
    {
        var now = DateTime.UtcNow;

        return new EmployeePosition
        {
            Id = Guid.NewGuid(),
            EmployeeId = employeeId,
            PositionId = positionId,
            DepartmentId = departmentId,
            StartDate = startDate,
            EndDate = null,            // เริ่มต้นเป็นตำแหน่งปัจจุบัน
            Salary = salary,
            Remark = remark,
            CreatedAt = now,
            UpdatedAt = now
        };
    }

    /// <summary>ปิดช่วงการดำรงตำแหน่ง (เช่น ตอนย้ายไปตำแหน่งใหม่)</summary>
    public void Close(DateOnly endDate)
    {
        if (EndDate is not null)
            throw new InvalidOperationException("ตำแหน่งนี้ถูกปิดไปแล้ว");
        if (endDate < StartDate)
            throw new InvalidOperationException("วันสิ้นสุดต้องไม่ก่อนวันเริ่ม");

        EndDate = endDate;
        Touch();
    }

    /// <summary>แก้ไขเงินเดือน/หมายเหตุ (ไม่แตะช่วงเวลาหรือตำแหน่ง)</summary>
    public void UpdateDetails(decimal salary, string? remark = null)
    {
        Salary = salary;
        Remark = remark;
        Touch();
    }

    private void Touch() => UpdatedAt = DateTime.UtcNow;
}
