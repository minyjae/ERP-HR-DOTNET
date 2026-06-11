using ERP.Domain.Entities;

namespace ERP.Application.Interfaces;

/// <summary>
/// สัญญาของชั้นเก็บข้อมูลประวัติตำแหน่ง — ประกาศใน Application แต่ implement จริงใน Infrastructure
/// </summary>
public interface IEmployeePositionRepository
{
    /// <summary>timeline ทั้งหมดของพนักงาน (เรียงใหม่ → เก่า)</summary>
    Task<List<EmployeePosition>> GetByEmployeeAsync(Guid employeeId, CancellationToken ct = default);

    /// <summary>ตำแหน่งปัจจุบัน (EndDate == null) ของพนักงาน — มีได้ทีละ 1</summary>
    Task<EmployeePosition?> GetCurrentByEmployeeAsync(Guid employeeId, CancellationToken ct = default);

    Task<EmployeePosition?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(EmployeePosition employeePosition, CancellationToken ct = default);
    void Remove(EmployeePosition employeePosition);

    /// <summary>commit การเปลี่ยนแปลงทั้งหมดลง database (unit of work)</summary>
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
