using ERP.Domain.Entities;

namespace ERP.Application.Interfaces;

/// <summary>สัญญาของชั้นเก็บข้อมูลนโยบายการลา — implement จริงใน Infrastructure</summary>
public interface ILeavePolicyRepository
{
    Task<List<LeavePolicy>> GetAllAsync(CancellationToken ct = default);
    Task<List<LeavePolicy>> GetByLeaveTypeAsync(Guid leaveTypeId, CancellationToken ct = default);
    Task<LeavePolicy?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>policy ที่ active ของประเภทลานี้ (มีได้ทีละ 1) — ใช้ดึงกฎ AdvanceNoticeDays ตอนยื่นลา</summary>
    Task<LeavePolicy?> GetActiveByLeaveTypeAsync(Guid leaveTypeId, CancellationToken ct = default);

    /// <summary>มี policy ที่ active อยู่แล้วของประเภทลานี้ไหม (กันไว้ให้มีได้ทีละ 1) — ข้าม id ตัวเองได้</summary>
    Task<bool> HasActivePolicyAsync(Guid leaveTypeId, Guid? excludeId, CancellationToken ct = default);

    Task AddAsync(LeavePolicy policy, CancellationToken ct = default);
    void Remove(LeavePolicy policy);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
