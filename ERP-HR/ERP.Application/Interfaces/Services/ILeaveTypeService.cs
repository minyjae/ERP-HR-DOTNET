using ERP.Application.DTOs;

namespace ERP.Application.Interfaces;

/// <summary>สัญญาของ business logic ฝั่งประเภทการลา</summary>
public interface ILeaveTypeService
{
    Task<List<LeaveTypeResponse>> GetAllAsync(CancellationToken ct = default);
    Task<LeaveTypeResponse?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<LeaveTypeResponse> CreateAsync(CreateLeaveTypeRequest request, CancellationToken ct = default);
    Task<LeaveTypeResponse> UpdateAsync(Guid id, UpdateLeaveTypeRequest request, CancellationToken ct = default);
    Task ActivateAsync(Guid id, CancellationToken ct = default);
    Task DeactivateAsync(Guid id, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
