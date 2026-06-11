using ERP.Application.DTOs;
using ERP.Domain.Entities;

namespace ERP.Application.Mappings;

/// <summary>แปลง LeaveAllocation entity → DTO (รวม RemainingDays ที่ domain คำนวณให้)</summary>
public static class LeaveAllocationMappingExtensions
{
    public static LeaveAllocationResponse ToResponse(this LeaveAllocation la) => new()
    {
        Id = la.Id,
        EmployeeId = la.EmployeeId,
        LeaveTypeId = la.LeaveTypeId,
        Year = la.Year,
        TotalDays = la.TotalDays,
        UsedDays = la.UsedDays,
        CarryOverDays = la.CarryOverDays,
        RemainingDays = la.RemainingDays,
        CreatedAt = la.CreatedAt,
        UpdatedAt = la.UpdatedAt
    };
}
