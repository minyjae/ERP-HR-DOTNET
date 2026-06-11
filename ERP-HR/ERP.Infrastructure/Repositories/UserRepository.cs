using ERP.Application.Interfaces;
using ERP.Domain.Entities;
using ERP.Domain.Enums;
using ERP.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Repositories;

/// <summary>
/// implement IUserRepository ด้วย EF Core
/// ที่นี่คือที่เดียวที่ "รู้จัก" DbContext จริง ๆ — ชั้นบน (service) ไม่เห็น EF เลย
/// </summary>
public class UserRepository(AppDbContext db) : IUserRepository
{
    public async Task<List<User>> GetAllAsync(CancellationToken ct = default) =>
        await db.Users
            .AsNoTracking()
            .OrderBy(u => u.Email)
            .ToListAsync(ct);

    public async Task<List<User>> GetByRoleAsync(UserRole role, CancellationToken ct = default) =>
        await db.Users
            .AsNoTracking()
            .Where(u => u.Role == role)
            .OrderBy(u => u.Email)
            .ToListAsync(ct);

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        // ไม่ใส่ AsNoTracking เพราะ caller อาจแก้ไขแล้ว SaveChanges
        await db.Users.FirstOrDefaultAsync(u => u.Id == id, ct);

    public Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default) =>
        db.Users.AnyAsync(u => u.Email == email, ct);

    public Task<bool> ExistsByEmployeeIdAsync(Guid employeeId, CancellationToken ct = default) =>
        db.Users.AnyAsync(u => u.EmployeeId == employeeId, ct);

    public async Task AddAsync(User user, CancellationToken ct = default) =>
        await db.Users.AddAsync(user, ct);

    public void Remove(User user) =>
        db.Users.Remove(user);

    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        db.SaveChangesAsync(ct);
}
