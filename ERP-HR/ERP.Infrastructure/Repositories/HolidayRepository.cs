using ERP.Application.Interfaces;
using ERP.Domain.Entities;
using ERP.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Repositories;

/// <summary>implement IHolidayRepository ด้วย EF Core</summary>
public class HolidayRepository(AppDbContext db) : IHolidayRepository
{
    public async Task<List<Holiday>> GetAllAsync(CancellationToken ct = default) =>
        await db.Holidays
            .AsNoTracking()
            .OrderBy(h => h.Date)
            .ToListAsync(ct);

    public async Task<List<Holiday>> GetByYearAsync(int year, CancellationToken ct = default) =>
        await db.Holidays
            .AsNoTracking()
            .Where(h => h.Year == year)
            .OrderBy(h => h.Date)
            .ToListAsync(ct);

    public async Task<Holiday?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await db.Holidays.FirstOrDefaultAsync(h => h.Id == id, ct);

    public Task<bool> ExistsByDateAsync(DateOnly date, CancellationToken ct = default) =>
        db.Holidays.AnyAsync(h => h.Date == date, ct);

    public async Task AddAsync(Holiday holiday, CancellationToken ct = default) =>
        await db.Holidays.AddAsync(holiday, ct);

    public void Remove(Holiday holiday) =>
        db.Holidays.Remove(holiday);

    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        db.SaveChangesAsync(ct);
}
