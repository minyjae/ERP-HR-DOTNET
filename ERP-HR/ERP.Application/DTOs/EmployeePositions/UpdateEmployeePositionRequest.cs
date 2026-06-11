namespace ERP.Application.DTOs;

/// <summary>
/// แก้ไขเงินเดือน/หมายเหตุของ record ประวัติตำแหน่ง (ไม่เปลี่ยนช่วงเวลาหรือตำแหน่ง)
/// </summary>
public record UpdateEmployeePositionRequest
{
    public required decimal Salary { get; init; }
    public string? Remark { get; init; }
}
