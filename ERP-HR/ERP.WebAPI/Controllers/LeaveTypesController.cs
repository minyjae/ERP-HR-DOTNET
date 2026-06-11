using ERP.Application.DTOs;
using ERP.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers;

/// <summary>ประเภทการลา — controller "บาง" error จัดการรวมที่ GlobalExceptionHandler</summary>
[ApiController]
[Route("api/[controller]")]
public class LeaveTypesController(ILeaveTypeService service) : ControllerBase
{
    /// <summary>ดึงประเภทลาทั้งหมด</summary>
    [HttpGet]
    [ProducesResponseType<List<LeaveTypeResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<LeaveTypeResponse>>> GetAll(CancellationToken ct)
    {
        var result = await service.GetAllAsync(ct);
        return Ok(result);
    }

    /// <summary>ดึงประเภทลาตาม id</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<LeaveTypeResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LeaveTypeResponse>> GetById(Guid id, CancellationToken ct)
    {
        var result = await service.GetByIdAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>สร้างประเภทลาใหม่</summary>
    [HttpPost]
    [ProducesResponseType<LeaveTypeResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<LeaveTypeResponse>> Create(CreateLeaveTypeRequest request, CancellationToken ct)
    {
        var created = await service.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>แก้ไขประเภทลา</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType<LeaveTypeResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LeaveTypeResponse>> Update(Guid id, UpdateLeaveTypeRequest request, CancellationToken ct)
    {
        var updated = await service.UpdateAsync(id, request, ct);
        return Ok(updated);
    }

    /// <summary>เปิดใช้งานประเภทลา</summary>
    [HttpPatch("{id:guid}/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Activate(Guid id, CancellationToken ct)
    {
        await service.ActivateAsync(id, ct);
        return NoContent();
    }

    /// <summary>ปิดใช้งานประเภทลา (soft — ไม่ลบข้อมูลจริง)</summary>
    [HttpPatch("{id:guid}/deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct)
    {
        await service.DeactivateAsync(id, ct);
        return NoContent();
    }

    /// <summary>ลบประเภทลา</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await service.DeleteAsync(id, ct);
        return NoContent();
    }
}
