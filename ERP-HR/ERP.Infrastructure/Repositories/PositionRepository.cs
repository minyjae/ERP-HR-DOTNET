using ERP.Application.Interfaces;
using ERP.Domain.Entities;
using ERP.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Repositories;

/// <summary>
/// implement IPositionRepository ด้วย EF Core
/// ที่นี่คือที่เดียวที่ "รู้จัก" DbContext จริง ๆ — ชั้นบน (service) ไม่เห็น EF เลย
/// </summary>
public class PositionRepository(AppDbContext db) : IPositionRepository
{
    public async Task<List<Position>> GetAllAsync(CancellationToken ct = default) =>
        await db.Positions
            .AsNoTracking()          // อ่านอย่างเดียว ไม่ต้อง track → เร็วกว่า
            .OrderBy(p => p.Code)
            .ToListAsync(ct);

    public async Task<Position?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        // ไม่ใส่ AsNoTracking เพราะ caller อาจแก้ไขแล้ว SaveChanges
        await db.Positions.FirstOrDefaultAsync(p => p.Id == id, ct);

    public Task<bool> ExistsByCodeAsync(string code, CancellationToken ct = default) =>
        db.Positions.AnyAsync(p => p.Code == code, ct);

    public async Task AddAsync(Position position, CancellationToken ct = default) =>
        await db.Positions.AddAsync(position, ct);

    public void Remove(Position position) =>
        db.Positions.Remove(position);

    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        db.SaveChangesAsync(ct);
}
