using ERP.Application.Interfaces;
using ERP.Domain.Entities;
using ERP.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Repositories;

/// <summary>implement ILeaveAllocationRepository ด้วย EF Core</summary>
public class LeaveAllocationRepository(AppDbContext db) : ILeaveAllocationRepository
{
    public async Task<List<LeaveAllocation>> GetByEmployeeAsync(Guid employeeId, int? year, CancellationToken ct = default) =>
        await db.LeaveAllocations
            .AsNoTracking()
            .Where(la => la.EmployeeId == employeeId)
            .Where(la => year == null || la.Year == year)
            .OrderByDescending(la => la.Year)
            .ToListAsync(ct);

    public async Task<LeaveAllocation?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await db.LeaveAllocations.FirstOrDefaultAsync(la => la.Id == id, ct);

    public async Task<LeaveAllocation?> GetByKeyAsync(Guid employeeId, Guid leaveTypeId, int year, CancellationToken ct = default) =>
        // ไม่ใส่ AsNoTracking เพราะ caller (อนุมัติ/ยกเลิกใบลา) ต้องแก้ไขแล้ว SaveChanges
        await db.LeaveAllocations.FirstOrDefaultAsync(
            la => la.EmployeeId == employeeId && la.LeaveTypeId == leaveTypeId && la.Year == year, ct);

    public Task<bool> ExistsAsync(Guid employeeId, Guid leaveTypeId, int year, CancellationToken ct = default) =>
        db.LeaveAllocations.AnyAsync(
            la => la.EmployeeId == employeeId && la.LeaveTypeId == leaveTypeId && la.Year == year, ct);

    public async Task AddAsync(LeaveAllocation allocation, CancellationToken ct = default) =>
        await db.LeaveAllocations.AddAsync(allocation, ct);

    public void Remove(LeaveAllocation allocation) =>
        db.LeaveAllocations.Remove(allocation);

    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        db.SaveChangesAsync(ct);
}
