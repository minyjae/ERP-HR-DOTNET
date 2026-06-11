using ERP.Application.Interfaces;
using ERP.Domain.Entities;
using ERP.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Repositories;

/// <summary>
/// implement IBranchRepository ด้วย EF Core
/// ที่นี่คือที่เดียวที่ "รู้จัก" DbContext จริง ๆ — ชั้นบน (service) ไม่เห็น EF เลย
/// </summary>
public class BranchRepository(AppDbContext db) : IBranchRepository
{
    public async Task<List<Branch>> GetAllAsync(CancellationToken ct = default) =>
        await db.Branches
            .AsNoTracking()          // อ่านอย่างเดียว ไม่ต้อง track → เร็วกว่า
            .OrderBy(b => b.Code)
            .ToListAsync(ct);

    public async Task<Branch?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        // ไม่ใส่ AsNoTracking เพราะ caller อาจแก้ไขแล้ว SaveChanges
        await db.Branches.FirstOrDefaultAsync(b => b.Id == id, ct);

    public Task<bool> ExistsByCodeAsync(string code, CancellationToken ct = default) =>
        db.Branches.AnyAsync(b => b.Code == code, ct);

    public async Task AddAsync(Branch branch, CancellationToken ct = default) =>
        await db.Branches.AddAsync(branch, ct);

    public void Remove(Branch branch) =>
        db.Branches.Remove(branch);

    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        db.SaveChangesAsync(ct);
}
