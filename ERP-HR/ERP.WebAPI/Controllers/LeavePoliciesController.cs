using ERP.Application.DTOs;
using ERP.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers;

/// <summary>นโยบายการลา — controller "บาง" error จัดการรวมที่ GlobalExceptionHandler</summary>
[ApiController]
[Route("api/[controller]")]
public class LeavePoliciesController(ILeavePolicyService service) : ControllerBase
{
    /// <summary>ดึงนโยบายทั้งหมด หรือกรองตามประเภทลาด้วย ?leaveTypeId=</summary>
    [HttpGet]
    [ProducesResponseType<List<LeavePolicyResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<LeavePolicyResponse>>> GetAll([FromQuery] Guid? leaveTypeId, CancellationToken ct)
    {
        var result = leaveTypeId is null
            ? await service.GetAllAsync(ct)
            : await service.GetByLeaveTypeAsync(leaveTypeId.Value, ct);
        return Ok(result);
    }

    /// <summary>ดึงนโยบายตาม id</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<LeavePolicyResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LeavePolicyResponse>> GetById(Guid id, CancellationToken ct)
    {
        var result = await service.GetByIdAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>สร้างนโยบายการลาใหม่</summary>
    [HttpPost]
    [ProducesResponseType<LeavePolicyResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<LeavePolicyResponse>> Create(CreateLeavePolicyRequest request, CancellationToken ct)
    {
        var created = await service.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>แก้ไขกฎเกณฑ์ของนโยบาย</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType<LeavePolicyResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LeavePolicyResponse>> Update(Guid id, UpdateLeavePolicyRequest request, CancellationToken ct)
    {
        var updated = await service.UpdateAsync(id, request, ct);
        return Ok(updated);
    }

    /// <summary>เปิดใช้งานนโยบาย (active ได้ทีละ 1 ต่อประเภทลา)</summary>
    [HttpPatch("{id:guid}/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Activate(Guid id, CancellationToken ct)
    {
        await service.ActivateAsync(id, ct);
        return NoContent();
    }

    /// <summary>ปิดใช้งานนโยบาย</summary>
    [HttpPatch("{id:guid}/deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct)
    {
        await service.DeactivateAsync(id, ct);
        return NoContent();
    }

    /// <summary>ลบนโยบาย</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await service.DeleteAsync(id, ct);
        return NoContent();
    }
}
