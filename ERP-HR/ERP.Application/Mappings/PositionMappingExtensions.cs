using ERP.Application.DTOs;
using ERP.Domain.Entities;

namespace ERP.Application.Mappings;

/// <summary>แปลง Position entity → DTO ด้วย extension method แบบเขียนมือ (สไตล์เดียวกับ Branch/Employee)</summary>
public static class PositionMappingExtensions
{
    public static PositionResponse ToResponse(this Position p) => new()
    {
        Id = p.Id,
        Code = p.Code,
        Name = p.Name,
        NameEn = p.NameEn,
        Level = p.Level,
        DepartmentId = p.DepartmentId,
        IsActive = p.IsActive,
        CreatedAt = p.CreatedAt,
        UpdatedAt = p.UpdatedAt
    };
}
