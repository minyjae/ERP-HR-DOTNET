using ERP.Application.DTOs;
using ERP.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers;

/// <summary>
/// โควต้าวันลาของพนักงาน — controller "บาง" error จัดการรวมที่ GlobalExceptionHandler
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class LeaveAllocationsController(ILeaveAllocationService service) : ControllerBase
{
    /// <summary>ดึงโควต้าของพนักงาน (กรองตามปีด้วย ?year=2026)</summary>
    [HttpGet]
    [ProducesResponseType<List<LeaveAllocationResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<LeaveAllocationResponse>>> GetByEmployee(
        [FromQuery] Guid employeeId, [FromQuery] int? year, CancellationToken ct)
    {
        var result = await service.GetByEmployeeAsync(employeeId, year, ct);
        return Ok(result);
    }

    /// <summary>ดึงโควต้าตาม id</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<LeaveAllocationResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LeaveAllocationResponse>> GetById(Guid id, CancellationToken ct)
    {
        var result = await service.GetByIdAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>สร้างโควต้าวันลาให้พนักงาน</summary>
    [HttpPost]
    [ProducesResponseType<LeaveAllocationResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<LeaveAllocationResponse>> Create(CreateLeaveAllocationRequest request, CancellationToken ct)
    {
        var created = await service.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>ปรับโควต้า/ยอดยกมา (ไม่แตะวันที่ใช้ไปแล้ว)</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType<LeaveAllocationResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LeaveAllocationResponse>> Update(Guid id, UpdateLeaveAllocationRequest request, CancellationToken ct)
    {
        var updated = await service.UpdateAsync(id, request, ct);
        return Ok(updated);
    }

    /// <summary>ลบโควต้าวันลา</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await service.DeleteAsync(id, ct);
        return NoContent();
    }
}
