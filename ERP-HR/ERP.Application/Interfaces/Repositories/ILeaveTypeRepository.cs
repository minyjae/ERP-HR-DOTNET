using ERP.Domain.Entities;

namespace ERP.Application.Interfaces;

/// <summary>สัญญาของชั้นเก็บข้อมูลประเภทการลา — implement จริงใน Infrastructure</summary>
public interface ILeaveTypeRepository
{
    Task<List<LeaveType>> GetAllAsync(CancellationToken ct = default);
    Task<LeaveType?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<bool> ExistsByCodeAsync(string code, CancellationToken ct = default);
    Task AddAsync(LeaveType leaveType, CancellationToken ct = default);
    void Remove(LeaveType leaveType);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
