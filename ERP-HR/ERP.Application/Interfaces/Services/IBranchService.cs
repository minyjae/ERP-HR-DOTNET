using ERP.Application.DTOs;

namespace ERP.Application.Interfaces;

/// <summary>
/// สัญญาของ business logic ฝั่ง Branch
/// controller คุยกับ interface นี้เท่านั้น ไม่ยุ่งกับ repository โดยตรง
/// </summary>
public interface IBranchService
{
    Task<List<BranchResponse>> GetAllAsync(CancellationToken ct = default);
    Task<BranchResponse?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BranchResponse> CreateAsync(CreateBranchRequest request, CancellationToken ct = default);
    Task<BranchResponse> UpdateAsync(Guid id, UpdateBranchRequest request, CancellationToken ct = default);
    Task ActivateAsync(Guid id, CancellationToken ct = default);
    Task DeactivateAsync(Guid id, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
