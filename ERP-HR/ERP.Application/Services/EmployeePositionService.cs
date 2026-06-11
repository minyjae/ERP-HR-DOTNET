using ERP.Application.Common.Exceptions;
using ERP.Application.DTOs;
using ERP.Application.Interfaces;
using ERP.Application.Mappings;
using ERP.Domain.Entities;
using FluentValidation;

namespace ERP.Application.Services;

/// <summary>
/// business logic ฝั่งประวัติตำแหน่ง
/// หัวใจคือ AssignAsync: ปิดตำแหน่งปัจจุบัน + เปิดตำแหน่งใหม่ ใน SaveChanges เดียว (atomic)
/// </summary>
public class EmployeePositionService(
    IEmployeePositionRepository repository,
    IValidator<AssignPositionRequest> assignValidator,
    IValidator<UpdateEmployeePositionRequest> updateValidator) : IEmployeePositionService
{
    public async Task<List<EmployeePositionResponse>> GetByEmployeeAsync(Guid employeeId, CancellationToken ct = default)
    {
        var items = await repository.GetByEmployeeAsync(employeeId, ct);
        return items.Select(ep => ep.ToResponse()).ToList();
    }

    public async Task<EmployeePositionResponse?> GetCurrentAsync(Guid employeeId, CancellationToken ct = default)
    {
        var current = await repository.GetCurrentByEmployeeAsync(employeeId, ct);
        return current?.ToResponse();
    }

    public async Task<EmployeePositionResponse> AssignAsync(AssignPositionRequest request, CancellationToken ct = default)
    {
        await assignValidator.ValidateAndThrowAsync(request, ct);

        // ปิดตำแหน่งปัจจุบัน (ถ้ามี) ก่อนเริ่มตำแหน่งใหม่
        var current = await repository.GetCurrentByEmployeeAsync(request.EmployeeId, ct);
        if (current is not null)
        {
            if (request.StartDate <= current.StartDate)
                throw new ConflictException("วันเริ่มตำแหน่งใหม่ต้องอยู่หลังวันเริ่มตำแหน่งปัจจุบัน");

            current.Close(request.StartDate.AddDays(-1));   // ปิดวันก่อนหน้าวันเริ่มใหม่ (ไม่ให้ช่วงทับกัน)
        }

        var assignment = EmployeePosition.Create(
            employeeId: request.EmployeeId,
            positionId: request.PositionId,
            departmentId: request.DepartmentId,
            startDate: request.StartDate,
            salary: request.Salary,
            remark: request.Remark);

        await repository.AddAsync(assignment, ct);
        await repository.SaveChangesAsync(ct);   // ปิดเก่า + เพิ่มใหม่ commit พร้อมกัน

        return assignment.ToResponse();
    }

    public async Task<EmployeePositionResponse> UpdateAsync(Guid id, UpdateEmployeePositionRequest request, CancellationToken ct = default)
    {
        await updateValidator.ValidateAndThrowAsync(request, ct);

        var item = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"ไม่พบประวัติตำแหน่ง id '{id}'");

        item.UpdateDetails(request.Salary, request.Remark);
        await repository.SaveChangesAsync(ct);
        return item.ToResponse();
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var item = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"ไม่พบประวัติตำแหน่ง id '{id}'");

        repository.Remove(item);
        await repository.SaveChangesAsync(ct);
    }
}
