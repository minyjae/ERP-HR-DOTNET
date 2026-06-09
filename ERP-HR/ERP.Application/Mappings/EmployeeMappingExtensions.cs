using ERP.Application.DTOs;
using ERP.Domain.Entities;

namespace ERP.Application.Mappings;

/// <summary>
/// แปลงระหว่าง entity กับ DTO ด้วย extension method แบบเขียนมือ
/// (ตั้งใจไม่ใช้ AutoMapper เพื่อให้เห็นการ map ตรง ๆ ตอนศึกษา)
/// </summary>
public static class EmployeeMappingExtensions
{
    public static EmployeeResponse ToResponse(this Employee e) => new()
    {
        Id = e.Id,
        EmployeeCode = e.EmployeeCode,
        FirstName = e.FirstName,
        LastName = e.LastName,
        FullName = $"{e.FirstName} {e.LastName}",
        FirstNameEn = e.FirstNameEn,
        LastNameEn = e.LastNameEn,
        DateOfBirth = e.DateOfBirth,
        Gender = e.Gender,
        NationalId = e.NationalId,
        PhoneNumber = e.PhoneNumber,
        Email = e.Email,
        Address = e.Address,
        ProfileImageUrl = e.ProfileImageUrl,
        HireDate = e.HireDate,
        Status = e.Status,
        DepartmentId = e.DepartmentId,
        PositionId = e.PositionId,
        BranchId = e.BranchId,
        ManagerId = e.ManagerId,
        CreatedAt = e.CreatedAt,
        UpdatedAt = e.UpdatedAt
    };
}
