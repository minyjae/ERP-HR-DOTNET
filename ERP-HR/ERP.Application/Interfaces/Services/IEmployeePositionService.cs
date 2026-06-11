using ERP.Application.DTOs;

namespace ERP.Application.Interfaces;

/// <summary>สัญญาของ business logic ฝั่งประวัติตำแหน่ง</summary>
public interface IEmployeePositionService
{
    Task<List<EmployeePositionResponse>> GetByEmployeeAsync(Guid employeeId, CancellationToken ct = default);
    Task<EmployeePositionResponse?> GetCurrentAsync(Guid employeeId, CancellationToken ct = default);

    /// <summary>มอบหมายตำแหน่งใหม่ — ปิดตำแหน่งปัจจุบันให้อัตโนมัติ</summary>
    Task<EmployeePositionResponse> AssignAsync(AssignPositionRequest request, CancellationToken ct = default);

    Task<EmployeePositionResponse> UpdateAsync(Guid id, UpdateEmployeePositionRequest request, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
