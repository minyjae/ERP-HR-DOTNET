using ERP.Application.Interfaces;
using ERP.Domain.Entities;
using ERP.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Repositories;

/// <summary>implement IEmployeePositionRepository ด้วย EF Core</summary>
public class EmployeePositionRepository(AppDbContext db) : IEmployeePositionRepository
{
    public async Task<List<EmployeePosition>> GetByEmployeeAsync(Guid employeeId, CancellationToken ct = default) =>
        await db.EmployeePositions
            .AsNoTracking()
            .Where(ep => ep.EmployeeId == employeeId)
            .OrderByDescending(ep => ep.StartDate)   // ล่าสุดอยู่บนสุด
            .ToListAsync(ct);

    public async Task<EmployeePosition?> GetCurrentByEmployeeAsync(Guid employeeId, CancellationToken ct = default) =>
        // ไม่ใส่ AsNoTracking เพราะ AssignAsync ต้องโหลดมาเพื่อ "ปิด" (แก้ไข)
        await db.EmployeePositions
            .FirstOrDefaultAsync(ep => ep.EmployeeId == employeeId && ep.EndDate == null, ct);

    public async Task<EmployeePosition?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await db.EmployeePositions.FirstOrDefaultAsync(ep => ep.Id == id, ct);

    public async Task AddAsync(EmployeePosition employeePosition, CancellationToken ct = default) =>
        await db.EmployeePositions.AddAsync(employeePosition, ct);

    public void Remove(EmployeePosition employeePosition) =>
        db.EmployeePositions.Remove(employeePosition);

    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        db.SaveChangesAsync(ct);
}
