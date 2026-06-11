using ERP.Domain.Entities;

namespace ERP.Application.Interfaces;

/// <summary>
/// สัญญาของชั้นเก็บข้อมูลตำแหน่ง — ประกาศใน Application แต่ implement จริงใน Infrastructure
/// (Application ไม่รู้จัก EF Core เลย → Dependency Inversion)
/// </summary>
public interface IPositionRepository
{
    Task<List<Position>> GetAllAsync(CancellationToken ct = default);
    Task<Position?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<bool> ExistsByCodeAsync(string code, CancellationToken ct = default);
    Task AddAsync(Position position, CancellationToken ct = default);
    void Remove(Position position);

    /// <summary>commit การเปลี่ยนแปลงทั้งหมดลง database (unit of work)</summary>
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
