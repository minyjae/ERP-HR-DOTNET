using ERP.Domain.Entities;

namespace ERP.Application.Interfaces;

/// <summary>สัญญาของชั้นเก็บข้อมูลโควต้าวันลา — implement จริงใน Infrastructure</summary>
public interface ILeaveAllocationRepository
{
    /// <summary>โควต้าทั้งหมดของพนักงาน (กรองตามปีได้)</summary>
    Task<List<LeaveAllocation>> GetByEmployeeAsync(Guid employeeId, int? year, CancellationToken ct = default);

    Task<LeaveAllocation?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>โควต้าของ (พนักงาน, ประเภทลา, ปี) แบบ tracked — สำหรับหัก/คืนตอนอนุมัติใบลา</summary>
    Task<LeaveAllocation?> GetByKeyAsync(Guid employeeId, Guid leaveTypeId, int year, CancellationToken ct = default);

    /// <summary>มีโควต้า (พนักงาน, ประเภทลา, ปี) นี้อยู่แล้วไหม — กันสร้างซ้ำ</summary>
    Task<bool> ExistsAsync(Guid employeeId, Guid leaveTypeId, int year, CancellationToken ct = default);

    Task AddAsync(LeaveAllocation allocation, CancellationToken ct = default);
    void Remove(LeaveAllocation allocation);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
