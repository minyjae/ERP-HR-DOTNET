using ERP.Application.DTOs;
using ERP.Domain.Entities;

namespace ERP.Application.Mappings;

/// <summary>แปลง LeaveType entity → DTO (สไตล์เดียวกับ Branch/Employee)</summary>
public static class LeaveTypeMappingExtensions
{
    public static LeaveTypeResponse ToResponse(this LeaveType lt) => new()
    {
        Id = lt.Id,
        Code = lt.Code,
        Name = lt.Name,
        IsPaid = lt.IsPaid,
        RequiresDocument = lt.RequiresDocument,
        IsActive = lt.IsActive,
        CreatedAt = lt.CreatedAt,
        UpdatedAt = lt.UpdatedAt
    };
}
