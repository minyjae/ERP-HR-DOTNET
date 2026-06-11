using ERP.Application.DTOs;
using ERP.Domain.Entities;

namespace ERP.Application.Mappings;

/// <summary>แปลง LeavePolicy entity → DTO (สไตล์เดียวกับ Branch/Employee)</summary>
public static class LeavePolicyMappingExtensions
{
    public static LeavePolicyResponse ToResponse(this LeavePolicy p) => new()
    {
        Id = p.Id,
        LeaveTypeId = p.LeaveTypeId,
        EntitledDays = p.EntitledDays,
        MaxCarryOverDays = p.MaxCarryOverDays,
        MinServiceMonths = p.MinServiceMonths,
        AdvanceNoticeDays = p.AdvanceNoticeDays,
        IsActive = p.IsActive,
        CreatedAt = p.CreatedAt,
        UpdatedAt = p.UpdatedAt
    };
}
