using ERP.Application.Interfaces;
using ERP.Domain.Entities;
using ERP.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Repositories;

/// <summary>implement ILeaveTypeRepository ด้วย EF Core</summary>
public class LeaveTypeRepository(AppDbContext db) : ILeaveTypeRepository
{
    public async Task<List<LeaveType>> GetAllAsync(CancellationToken ct = default) =>
        await db.LeaveTypes
            .AsNoTracking()
            .OrderBy(lt => lt.Code)
            .ToListAsync(ct);

    public async Task<LeaveType?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await db.LeaveTypes.FirstOrDefaultAsync(lt => lt.Id == id, ct);

    public Task<bool> ExistsByCodeAsync(string code, CancellationToken ct = default) =>
        db.LeaveTypes.AnyAsync(lt => lt.Code == code, ct);

    public async Task AddAsync(LeaveType leaveType, CancellationToken ct = default) =>
        await db.LeaveTypes.AddAsync(leaveType, ct);

    public void Remove(LeaveType leaveType) =>
        db.LeaveTypes.Remove(leaveType);

    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        db.SaveChangesAsync(ct);
}
