using ERP.Application.Common.Exceptions;
using ERP.Application.DTOs;
using ERP.Application.Interfaces;
using ERP.Application.Mappings;
using ERP.Domain.Entities;
using FluentValidation;

namespace ERP.Application.Services;

/// <summary>
/// business logic ฝั่ง Holiday: validate → ตรวจกฎ (วันซ้ำ) → เรียก domain → repo บันทึก
/// </summary>
public class HolidayService(
    IHolidayRepository repository,
    IValidator<CreateHolidayRequest> createValidator,
    IValidator<UpdateHolidayRequest> updateValidator) : IHolidayService
{
    public async Task<List<HolidayResponse>> GetAllAsync(CancellationToken ct = default)
    {
        var holidays = await repository.GetAllAsync(ct);
        return holidays.Select(h => h.ToResponse()).ToList();
    }

    public async Task<List<HolidayResponse>> GetByYearAsync(int year, CancellationToken ct = default)
    {
        var holidays = await repository.GetByYearAsync(year, ct);
        return holidays.Select(h => h.ToResponse()).ToList();
    }

    public async Task<HolidayResponse?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var holiday = await repository.GetByIdAsync(id, ct);
        return holiday?.ToResponse();
    }

    public async Task<HolidayResponse> CreateAsync(CreateHolidayRequest request, CancellationToken ct = default)
    {
        await createValidator.ValidateAndThrowAsync(request, ct);

        // กฎทางธุรกิจ: ห้ามมีวันหยุดซ้ำวันเดียวกัน
        if (await repository.ExistsByDateAsync(request.Date, ct))
            throw new ConflictException($"มีวันหยุดวันที่ {request.Date:yyyy-MM-dd} อยู่แล้ว");

        var holiday = Holiday.Create(
            name: request.Name,
            date: request.Date,
            isRecurring: request.IsRecurring);

        await repository.AddAsync(holiday, ct);
        await repository.SaveChangesAsync(ct);

        return holiday.ToResponse();
    }

    public async Task<HolidayResponse> UpdateAsync(Guid id, UpdateHolidayRequest request, CancellationToken ct = default)
    {
        await updateValidator.ValidateAndThrowAsync(request, ct);

        var holiday = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"ไม่พบวันหยุด id '{id}'");

        // ถ้าเปลี่ยนวันที่ ต้องไม่ชนกับวันหยุดอื่น
        if (request.Date != holiday.Date && await repository.ExistsByDateAsync(request.Date, ct))
            throw new ConflictException($"มีวันหยุดวันที่ {request.Date:yyyy-MM-dd} อยู่แล้ว");

        holiday.UpdateDetails(request.Name, request.Date, request.IsRecurring);
        await repository.SaveChangesAsync(ct);
        return holiday.ToResponse();
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var holiday = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"ไม่พบวันหยุด id '{id}'");

        repository.Remove(holiday);
        await repository.SaveChangesAsync(ct);
    }
}
