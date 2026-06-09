using ERP.Application.DTOs;
using ERP.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers;

/// <summary>
/// ชั้นบนสุด: รับ HTTP request → เรียก service → คืน HTTP response
/// controller "บาง" มาก ไม่มี business logic (อยู่ใน service หมด) และไม่มี try-catch
/// (error ถูกจัดการรวมที่ GlobalExceptionHandler)
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EmployeesController(IEmployeeService service) : ControllerBase
{
    /// <summary>ดึงพนักงานทั้งหมด</summary>
    [HttpGet]
    [ProducesResponseType<List<EmployeeResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<EmployeeResponse>>> GetAll(CancellationToken ct)
    {
        var result = await service.GetAllAsync(ct);
        return Ok(result);
    }

    /// <summary>ดึงพนักงานตาม id</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<EmployeeResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeResponse>> GetById(Guid id, CancellationToken ct)
    {
        var result = await service.GetByIdAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>สร้างพนักงานใหม่</summary>
    [HttpPost]
    [ProducesResponseType<EmployeeResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<EmployeeResponse>> Create(CreateEmployeeRequest request, CancellationToken ct)
    {
        var created = await service.CreateAsync(request, ct);
        // คืน 201 พร้อม Location header ชี้ไปที่ resource ที่เพิ่งสร้าง
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>แก้ไขข้อมูลพนักงาน</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType<EmployeeResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<EmployeeResponse>> Update(Guid id, UpdateEmployeeRequest request, CancellationToken ct)
    {
        var updated = await service.UpdateAsync(id, request, ct);
        return Ok(updated);
    }

    /// <summary>เปลี่ยนสถานะเป็น "ลาออก"</summary>
    [HttpPatch("{id:guid}/resign")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Resign(Guid id, CancellationToken ct)
    {
        await service.ResignAsync(id, ct);
        return NoContent();
    }

    /// <summary>ลบพนักงาน</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await service.DeleteAsync(id, ct);
        return NoContent();
    }
}
