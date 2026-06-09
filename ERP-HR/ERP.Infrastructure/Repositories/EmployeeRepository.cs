using ERP.Application.Interfaces;
using ERP.Domain.Entities;
using ERP.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Repositories;

/// <summary>
/// implement IEmployeeRepository ด้วย EF Core
/// ที่นี่คือที่เดียวที่ "รู้จัก" DbContext จริง ๆ — ชั้นบน (service) ไม่เห็น EF เลย
/// </summary>
public class EmployeeRepository(AppDbContext db) : IEmployeeRepository
{
    public async Task<List<Employee>> GetAllAsync(CancellationToken ct = default) =>
        await db.Employees
            .AsNoTracking()          // อ่านอย่างเดียว ไม่ต้อง track → เร็วกว่า
            .OrderBy(e => e.EmployeeCode)
            .ToListAsync(ct);

    public async Task<Employee?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        // ไม่ใส่ AsNoTracking เพราะ caller อาจแก้ไขแล้ว SaveChanges
        await db.Employees.FirstOrDefaultAsync(e => e.Id == id, ct);

    public Task<bool> ExistsByEmployeeCodeAsync(string employeeCode, CancellationToken ct = default) =>
        db.Employees.AnyAsync(e => e.EmployeeCode == employeeCode, ct);

    public Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default) =>
        db.Employees.AnyAsync(e => e.Email == email, ct);

    public Task<bool> ExistsByNationalIdAsync(string nationalId, CancellationToken ct = default) =>
        db.Employees.AnyAsync(e => e.NationalId == nationalId, ct);

    public async Task AddAsync(Employee employee, CancellationToken ct = default) =>
        await db.Employees.AddAsync(employee, ct);

    public void Remove(Employee employee) =>
        db.Employees.Remove(employee);

    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        db.SaveChangesAsync(ct);
}
