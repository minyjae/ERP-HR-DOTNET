using ERP.Application.DTOs;
using ERP.Domain.Entities;

namespace ERP.Application.Mappings;

/// <summary>แปลง Department entity → DTO ด้วย extension method แบบเขียนมือ (สไตล์เดียวกับ Employee)</summary>
public static class DepartmentMappingExtensions
{
    public static DepartmentResponse ToResponse(this Department d) => new()
    {
        Id = d.Id,
        Code = d.Code,
        Name = d.Name,
        NameEn = d.NameEn,
        ManagerId = d.ManagerId,
        ParentDepartmentId = d.ParentDepartmentId,
        IsActive = d.IsActive,
        CreatedAt = d.CreatedAt,
        UpdatedAt = d.UpdatedAt
    };
}