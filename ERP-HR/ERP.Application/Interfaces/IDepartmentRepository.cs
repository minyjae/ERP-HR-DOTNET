using ERP.Domain.Entities;

namespace ERP.Application.Interfaces;

/// <summary>
/// สัญญาของชั้นเก็บข้อมูลแผนก — ประกาศใน Application แต่ implement จริงใน Infrastructure
/// (Application ไม่รู้จัก EF Core เลย → Dependency Inversion)
/// </summary>
public interface IDepartmentRepository
{
    Task<List<Department>> GetAllAsync(CancellationToken ct = default);
    Task<Department?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<bool> ExistsByCodeAsync(string code, CancellationToken ct = default);
    Task AddAsync(Department department, CancellationToken ct = default);
    void Remove(Department department);

    /// <summary>commit การเปลี่ยนแปลงทั้งหมดลง database (unit of work)</summary>
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}