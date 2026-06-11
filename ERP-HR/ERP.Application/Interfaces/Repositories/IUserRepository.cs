using ERP.Domain.Entities;
using ERP.Domain.Enums;

namespace ERP.Application.Interfaces;

/// <summary>
/// สัญญาของชั้นเก็บข้อมูลผู้ใช้ — ประกาศใน Application แต่ implement จริงใน Infrastructure
/// </summary>
public interface IUserRepository
{
    Task<List<User>> GetAllAsync(CancellationToken ct = default);
    Task<List<User>> GetByRoleAsync(UserRole role, CancellationToken ct = default);
    Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default);
    Task<bool> ExistsByEmployeeIdAsync(Guid employeeId, CancellationToken ct = default);
    Task AddAsync(User user, CancellationToken ct = default);
    void Remove(User user);

    /// <summary>commit การเปลี่ยนแปลงทั้งหมดลง database (unit of work)</summary>
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
