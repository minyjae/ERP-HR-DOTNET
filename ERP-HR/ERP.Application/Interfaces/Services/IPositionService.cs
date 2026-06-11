using ERP.Application.DTOs;

namespace ERP.Application.Interfaces;

/// <summary>
/// สัญญาของ business logic ฝั่ง Position
/// controller คุยกับ interface นี้เท่านั้น ไม่ยุ่งกับ repository โดยตรง
/// </summary>
public interface IPositionService
{
    Task<List<PositionResponse>> GetAllAsync(CancellationToken ct = default);
    Task<PositionResponse?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<PositionResponse> CreateAsync(CreatePositionRequest request, CancellationToken ct = default);
    Task<PositionResponse> UpdateAsync(Guid id, UpdatePositionRequest request, CancellationToken ct = default);
    Task ActivateAsync(Guid id, CancellationToken ct = default);
    Task DeactivateAsync(Guid id, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
