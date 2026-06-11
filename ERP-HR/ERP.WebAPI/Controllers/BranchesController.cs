using ERP.Application.DTOs;
using ERP.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers;

/// <summary>
/// ชั้นบนสุด: รับ HTTP request → เรียก service → คืน HTTP response
/// controller "บาง" — ไม่มี business logic และไม่มี try-catch (error จัดการรวมที่ GlobalExceptionHandler)
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BranchesController(IBranchService service) : ControllerBase
{
    /// <summary>ดึงสาขาทั้งหมด</summary>
    [HttpGet]
    [ProducesResponseType<List<BranchResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<BranchResponse>>> GetAll(CancellationToken ct)
    {
        var result = await service.GetAllAsync(ct);
        return Ok(result);
    }

    /// <summary>ดึงสาขาตาม id</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<BranchResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BranchResponse>> GetById(Guid id, CancellationToken ct)
    {
        var result = await service.GetByIdAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>สร้างสาขาใหม่</summary>
    [HttpPost]
    [ProducesResponseType<BranchResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<BranchResponse>> Create(CreateBranchRequest request, CancellationToken ct)
    {
        var created = await service.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>แก้ไขข้อมูลสาขา</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType<BranchResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BranchResponse>> Update(Guid id, UpdateBranchRequest request, CancellationToken ct)
    {
        var updated = await service.UpdateAsync(id, request, ct);
        return Ok(updated);
    }

    /// <summary>เปิดใช้งานสาขา</summary>
    [HttpPatch("{id:guid}/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Activate(Guid id, CancellationToken ct)
    {
        await service.ActivateAsync(id, ct);
        return NoContent();
    }

    /// <summary>ปิดใช้งานสาขา (soft — ไม่ลบข้อมูลจริง)</summary>
    [HttpPatch("{id:guid}/deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct)
    {
        await service.DeactivateAsync(id, ct);
        return NoContent();
    }

    /// <summary>ลบสาขา</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await service.DeleteAsync(id, ct);
        return NoContent();
    }
}
