using ERP.Application.DTOs;

namespace ERP.Application.Interfaces;

/// <summary>สัญญาของ business logic ฝั่งนโยบายการลา</summary>
public interface ILeavePolicyService
{
    Task<List<LeavePolicyResponse>> GetAllAsync(CancellationToken ct = default);
    Task<List<LeavePolicyResponse>> GetByLeaveTypeAsync(Guid leaveTypeId, CancellationToken ct = default);
    Task<LeavePolicyResponse?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<LeavePolicyResponse> CreateAsync(CreateLeavePolicyRequest request, CancellationToken ct = default);
    Task<LeavePolicyResponse> UpdateAsync(Guid id, UpdateLeavePolicyRequest request, CancellationToken ct = default);
    Task ActivateAsync(Guid id, CancellationToken ct = default);
    Task DeactivateAsync(Guid id, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
