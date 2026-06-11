using ERP.Domain.Entities;

namespace ERP.Application.Interfaces;

/// <summary>
/// สัญญาของชั้นเก็บข้อมูลสาขา — ประกาศใน Application แต่ implement จริงใน Infrastructure
/// (Application ไม่รู้จัก EF Core เลย → Dependency Inversion)
/// </summary>
public interface IBranchRepository
{
    Task<List<Branch>> GetAllAsync(CancellationToken ct = default);
    Task<Branch?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<bool> ExistsByCodeAsync(string code, CancellationToken ct = default);
    Task AddAsync(Branch branch, CancellationToken ct = default);
    void Remove(Branch branch);

    /// <summary>commit การเปลี่ยนแปลงทั้งหมดลง database (unit of work)</summary>
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
