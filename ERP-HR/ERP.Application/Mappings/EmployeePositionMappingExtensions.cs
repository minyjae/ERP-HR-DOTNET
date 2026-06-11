using ERP.Application.DTOs;
using ERP.Domain.Entities;

namespace ERP.Application.Mappings;

/// <summary>แปลง EmployeePosition entity → DTO (สไตล์เดียวกับ Branch/Employee)</summary>
public static class EmployeePositionMappingExtensions
{
    public static EmployeePositionResponse ToResponse(this EmployeePosition ep) => new()
    {
        Id = ep.Id,
        EmployeeId = ep.EmployeeId,
        PositionId = ep.PositionId,
        DepartmentId = ep.DepartmentId,
        StartDate = ep.StartDate,
        EndDate = ep.EndDate,
        Salary = ep.Salary,
        Remark = ep.Remark,
        IsCurrent = ep.IsCurrent,
        CreatedAt = ep.CreatedAt,
        UpdatedAt = ep.UpdatedAt
    };
}
