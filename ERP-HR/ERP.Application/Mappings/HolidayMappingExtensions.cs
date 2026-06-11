using ERP.Application.DTOs;
using ERP.Domain.Entities;

namespace ERP.Application.Mappings;

/// <summary>แปลง Holiday entity → DTO (สไตล์เดียวกับ Branch/Employee)</summary>
public static class HolidayMappingExtensions
{
    public static HolidayResponse ToResponse(this Holiday h) => new()
    {
        Id = h.Id,
        Name = h.Name,
        Date = h.Date,
        Year = h.Year,
        IsRecurring = h.IsRecurring,
        CreatedAt = h.CreatedAt,
        UpdatedAt = h.UpdatedAt
    };
}
