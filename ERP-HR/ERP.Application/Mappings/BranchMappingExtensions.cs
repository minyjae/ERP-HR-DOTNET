using ERP.Application.DTOs;
using ERP.Domain.Entities;

namespace ERP.Application.Mappings;

/// <summary>แปลง Branch entity → DTO ด้วย extension method แบบเขียนมือ (สไตล์เดียวกับ Employee/Department)</summary>
public static class BranchMappingExtensions
{
    public static BranchResponse ToResponse(this Branch b) => new()
    {
        Id = b.Id,
        Code = b.Code,
        Name = b.Name,
        Address = b.Address,
        Phone = b.Phone,
        IsActive = b.IsActive,
        CreatedAt = b.CreatedAt,
        UpdatedAt = b.UpdatedAt
    };
}
