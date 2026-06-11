using ERP.Application.DTOs;

namespace ERP.Application.Interfaces;

/// <summary>
/// สัญญาของ business logic ฝั่ง Department
/// controller คุยกับ interface นี้เท่านั้น ไม่ยุ่งกับ repository โดยตรง
/// </summary>
public interface IDepartmentService
{
    Task<List<DepartmentResponse>> GetAllAsync(CancellationToken ct = default);
    Task<DepartmentResponse?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<DepartmentResponse> CreateAsync(CreateDepartmentRequest request, CancellationToken ct = default);
    Task<DepartmentResponse> UpdateAsync(Guid id, UpdateDepartmentRequest request, CancellationToken ct = default);
    Task ActivateAsync(Guid id, CancellationToken ct = default);
    Task DeactivateAsync(Guid id, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}