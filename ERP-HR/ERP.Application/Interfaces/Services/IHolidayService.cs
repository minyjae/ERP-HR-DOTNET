using ERP.Application.DTOs;

namespace ERP.Application.Interfaces;

/// <summary>สัญญาของ business logic ฝั่งวันหยุด</summary>
public interface IHolidayService
{
    Task<List<HolidayResponse>> GetAllAsync(CancellationToken ct = default);
    Task<List<HolidayResponse>> GetByYearAsync(int year, CancellationToken ct = default);
    Task<HolidayResponse?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<HolidayResponse> CreateAsync(CreateHolidayRequest request, CancellationToken ct = default);
    Task<HolidayResponse> UpdateAsync(Guid id, UpdateHolidayRequest request, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
