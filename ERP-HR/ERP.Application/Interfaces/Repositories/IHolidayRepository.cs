using ERP.Domain.Entities;

namespace ERP.Application.Interfaces;

/// <summary>สัญญาของชั้นเก็บข้อมูลวันหยุด — implement จริงใน Infrastructure</summary>
public interface IHolidayRepository
{
    Task<List<Holiday>> GetAllAsync(CancellationToken ct = default);
    Task<List<Holiday>> GetByYearAsync(int year, CancellationToken ct = default);

    /// <summary>วันหยุดในช่วง [from, to] — ใช้ตอนคำนวณจำนวนวันลา</summary>
    Task<List<Holiday>> GetBetweenAsync(DateOnly from, DateOnly to, CancellationToken ct = default);

    Task<Holiday?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>มีวันหยุดในวันนี้อยู่แล้วไหม (กันบันทึกซ้ำ)</summary>
    Task<bool> ExistsByDateAsync(DateOnly date, CancellationToken ct = default);

    Task AddAsync(Holiday holiday, CancellationToken ct = default);
    void Remove(Holiday holiday);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
