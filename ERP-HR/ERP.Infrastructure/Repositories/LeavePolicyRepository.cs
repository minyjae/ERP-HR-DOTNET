using ERP.Application.Interfaces;
using ERP.Domain.Entities;
using ERP.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Repositories;

/// <summary>implement ILeavePolicyRepository ด้วย EF Core</summary>
public class LeavePolicyRepository(AppDbContext db) : ILeavePolicyRepository
{
    public async Task<List<LeavePolicy>> GetAllAsync(CancellationToken ct = default) =>
        await db.LeavePolicies
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task<List<LeavePolicy>> GetByLeaveTypeAsync(Guid leaveTypeId, CancellationToken ct = default) =>
        await db.LeavePolicies
            .AsNoTracking()
            .Where(p => p.LeaveTypeId == leaveTypeId)
            .ToListAsync(ct);

    public async Task<LeavePolicy?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await db.LeavePolicies.FirstOrDefaultAsync(p => p.Id == id, ct);

    public Task<bool> HasActivePolicyAsync(Guid leaveTypeId, Guid? excludeId, CancellationToken ct = default) =>
        db.LeavePolicies.AnyAsync(
            p => p.LeaveTypeId == leaveTypeId
                 && p.IsActive
                 && (excludeId == null || p.Id != excludeId), ct);

    public async Task AddAsync(LeavePolicy policy, CancellationToken ct = default) =>
        await db.LeavePolicies.AddAsync(policy, ct);

    public void Remove(LeavePolicy policy) =>
        db.LeavePolicies.Remove(policy);

    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        db.SaveChangesAsync(ct);
}
