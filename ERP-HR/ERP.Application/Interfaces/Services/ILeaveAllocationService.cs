using ERP.Application.DTOs;

namespace ERP.Application.Interfaces;

/// <summary>สัญญาของ business logic ฝั่งโควต้าวันลา</summary>
public interface ILeaveAllocationService
{
    Task<List<LeaveAllocationResponse>> GetByEmployeeAsync(Guid employeeId, int? year, CancellationToken ct = default);
    Task<LeaveAllocationResponse?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<LeaveAllocationResponse> CreateAsync(CreateLeaveAllocationRequest request, CancellationToken ct = default);
    Task<LeaveAllocationResponse> UpdateAsync(Guid id, UpdateLeaveAllocationRequest request, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
