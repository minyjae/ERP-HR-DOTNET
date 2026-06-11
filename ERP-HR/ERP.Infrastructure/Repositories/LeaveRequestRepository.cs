using ERP.Application.Interfaces;
using ERP.Domain.Entities;
using ERP.Domain.Enums;
using ERP.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Repositories;

/// <summary>implement ILeaveRequestRepository ด้วย EF Core</summary>
public class LeaveRequestRepository(AppDbContext db) : ILeaveRequestRepository
{
    public async Task<List<LeaveRequest>> GetByEmployeeAsync(
        Guid employeeId, LeaveRequestStatus? status, int? year, CancellationToken ct = default)
    {
        var query = db.LeaveRequests
            .AsNoTracking()
            .Where(r => r.EmployeeId == employeeId);

        if (status is not null)
            query = query.Where(r => r.Status == status);

        if (year is not null)
        {
            var from = new DateOnly(year.Value, 1, 1);
            var to = new DateOnly(year.Value, 12, 31);
            query = query.Where(r => r.StartDate >= from && r.StartDate <= to);
        }

        return await query.OrderByDescending(r => r.StartDate).ToListAsync(ct);
    }

    public async Task<LeaveRequest?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        // ไม่ใส่ AsNoTracking เพราะ caller (approve/reject/cancel) ต้องแก้ไขแล้ว SaveChanges
        await db.LeaveRequests.FirstOrDefaultAsync(r => r.Id == id, ct);

    public Task<bool> HasOverlapAsync(
        Guid employeeId, DateOnly start, DateOnly end, Guid? excludeId, CancellationToken ct = default) =>
        db.LeaveRequests.AnyAsync(r =>
            r.EmployeeId == employeeId
            && (r.Status == LeaveRequestStatus.Pending || r.Status == LeaveRequestStatus.Approved)
            && r.StartDate <= end && r.EndDate >= start          // ช่วงทับกัน
            && (excludeId == null || r.Id != excludeId), ct);

    public async Task AddAsync(LeaveRequest leaveRequest, CancellationToken ct = default) =>
        await db.LeaveRequests.AddAsync(leaveRequest, ct);

    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        db.SaveChangesAsync(ct);
}
