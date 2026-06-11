using ERP.Application.DTOs;
using ERP.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers;

/// <summary>
/// ชั้นบนสุด: รับ HTTP request → เรียก service → คืน HTTP response
/// controller "บาง" — error จัดการรวมที่ GlobalExceptionHandler
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HolidaysController(IHolidayService service) : ControllerBase
{
    /// <summary>ดึงวันหยุดทั้งหมด หรือกรองตามปีด้วย ?year=2026</summary>
    [HttpGet]
    [ProducesResponseType<List<HolidayResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<HolidayResponse>>> GetAll([FromQuery] int? year, CancellationToken ct)
    {
        var result = year is null
            ? await service.GetAllAsync(ct)
            : await service.GetByYearAsync(year.Value, ct);
        return Ok(result);
    }

    /// <summary>ดึงวันหยุดตาม id</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<HolidayResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<HolidayResponse>> GetById(Guid id, CancellationToken ct)
    {
        var result = await service.GetByIdAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>สร้างวันหยุดใหม่</summary>
    [HttpPost]
    [ProducesResponseType<HolidayResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<HolidayResponse>> Create(CreateHolidayRequest request, CancellationToken ct)
    {
        var created = await service.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>แก้ไขวันหยุด</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType<HolidayResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<HolidayResponse>> Update(Guid id, UpdateHolidayRequest request, CancellationToken ct)
    {
        var updated = await service.UpdateAsync(id, request, ct);
        return Ok(updated);
    }

    /// <summary>ลบวันหยุด</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await service.DeleteAsync(id, ct);
        return NoContent();
    }
}
