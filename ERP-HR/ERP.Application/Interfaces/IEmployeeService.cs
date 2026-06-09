using ERP.Application.DTOs;

namespace ERP.Application.Interfaces;

/// <summary>
/// สัญญาของ business logic ฝั่ง Employee
/// controller จะคุยกับ interface นี้เท่านั้น ไม่ยุ่งกับ repository โดยตรง
/// </summary>
public interface IEmployeeService
{
    Task<List<EmployeeResponse>> GetAllAsync(CancellationToken ct = default);
    Task<EmployeeResponse?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<EmployeeResponse> CreateAsync(CreateEmployeeRequest request, CancellationToken ct = default);
    Task<EmployeeResponse> UpdateAsync(Guid id, UpdateEmployeeRequest request, CancellationToken ct = default);
    Task ResignAsync(Guid id, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
