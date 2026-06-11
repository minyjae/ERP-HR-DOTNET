using ERP.Domain.Entities;
using ERP.Domain.Enums;

namespace ERP.Application.Interfaces;

/// <summary>สัญญาของชั้นเก็บข้อมูลใบลา — implement จริงใน Infrastructure</summary>
public interface ILeaveRequestRepository
{
    Task<List<LeaveRequest>> GetByEmployeeAsync(Guid employeeId, LeaveRequestStatus? status, int? year, CancellationToken ct = default);
    Task<LeaveRequest?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>มีใบลา (Pending/Approved) ของพนักงานที่ช่วงวันทับกับ [start, end] ไหม — ข้าม id ตัวเองได้</summary>
    Task<bool> HasOverlapAsync(Guid employeeId, DateOnly start, DateOnly end, Guid? excludeId, CancellationToken ct = default);

    Task AddAsync(LeaveRequest leaveRequest, CancellationToken ct = default);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
