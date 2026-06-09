using ERP.Application.Interfaces;
using ERP.Domain.Entities;
using ERP.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Repositories;

/// <summary>
/// implement IDepartmentRepository ด้วย EF Core
/// ที่นี่คือที่เดียวที่ "รู้จัก" DbContext จริง ๆ — ชั้นบน (service) ไม่เห็น EF เลย
/// </summary>
public class DepartmentRepository(AppDbContext db) : IDepartmentRepository
{
    public async Task<List<Department>> GetAllAsync(CancellationToken ct = default) =>
        await db.Departments
            .AsNoTracking()          // อ่านอย่างเดียว ไม่ต้อง track → เร็วกว่า
            .OrderBy(d => d.Code)
            .ToListAsync(ct);

    public async Task<Department?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        // ไม่ใส่ AsNoTracking เพราะ caller อาจแก้ไขแล้ว SaveChanges
        await db.Departments.FirstOrDefaultAsync(d => d.Id == id, ct);

    public Task<bool> ExistsByCodeAsync(string code, CancellationToken ct = default) =>
        db.Departments.AnyAsync(d => d.Code == code, ct);

    public async Task AddAsync(Department department, CancellationToken ct = default) =>
        await db.Departments.AddAsync(department, ct);

    public void Remove(Department department) =>
        db.Departments.Remove(department);

    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        db.SaveChangesAsync(ct);
}
