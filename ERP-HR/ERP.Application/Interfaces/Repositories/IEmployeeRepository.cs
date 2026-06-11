using ERP.Domain.Entities;

namespace ERP.Application.Interfaces;

/// <summary>
/// สัญญาของชั้นเก็บข้อมูล (repository)
/// ประกาศไว้ใน Application แต่ implement จริงใน Infrastructure
/// → Application ไม่รู้จัก EF Core เลย (Dependency Inversion)
/// </summary>
public interface IEmployeeRepository
{
    Task<List<Employee>> GetAllAsync(CancellationToken ct = default);
    Task<Employee?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<bool> ExistsByEmployeeCodeAsync(string employeeCode, CancellationToken ct = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default);
    Task<bool> ExistsByNationalIdAsync(string nationalId, CancellationToken ct = default);
    Task AddAsync(Employee employee, CancellationToken ct = default);
    void Remove(Employee employee);

    /// <summary>commit การเปลี่ยนแปลงทั้งหมดลง database (unit of work)</summary>
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
