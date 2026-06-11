using ERP.Application.DTOs;
using ERP.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers;

/// <summary>
/// ประวัติตำแหน่งของพนักงาน — ผูกใต้ resource employee (nested route)
/// controller "บาง" — error จัดการรวมที่ GlobalExceptionHandler
/// </summary>
[ApiController]
[Route("api/employees/{employeeId:guid}/positions")]
public class EmployeePositionsController(IEmployeePositionService service) : ControllerBase
{
    /// <summary>timeline ตำแหน่งทั้งหมดของพนักงาน (ใหม่ → เก่า)</summary>
    [HttpGet]
    [ProducesResponseType<List<EmployeePositionResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<EmployeePositionResponse>>> GetByEmployee(Guid employeeId, CancellationToken ct)
    {
        var result = await service.GetByEmployeeAsync(employeeId, ct);
        return Ok(result);
    }

    /// <summary>ตำแหน่งปัจจุบันของพนักงาน</summary>
    [HttpGet("current")]
    [ProducesResponseType<EmployeePositionResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeePositionResponse>> GetCurrent(Guid employeeId, CancellationToken ct)
    {
        var result = await service.GetCurrentAsync(employeeId, ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>มอบหมายตำแหน่งใหม่ (ปิดตำแหน่งปัจจุบันอัตโนมัติ)</summary>
    [HttpPost]
    [ProducesResponseType<EmployeePositionResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<EmployeePositionResponse>> Assign(Guid employeeId, AssignPositionRequest request, CancellationToken ct)
    {
        // กัน employeeId ใน path กับ body ไม่ตรงกัน
        if (employeeId != request.EmployeeId)
            return BadRequest("employeeId ใน path ไม่ตรงกับใน body");

        var created = await service.AssignAsync(request, ct);
        return CreatedAtAction(nameof(GetCurrent), new { employeeId }, created);
    }

    /// <summary>แก้ไขเงินเดือน/หมายเหตุของ record</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType<EmployeePositionResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeePositionResponse>> Update(Guid employeeId, Guid id, UpdateEmployeePositionRequest request, CancellationToken ct)
    {
        var updated = await service.UpdateAsync(id, request, ct);
        return Ok(updated);
    }

    /// <summary>ลบ record ประวัติตำแหน่ง</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid employeeId, Guid id, CancellationToken ct)
    {
        await service.DeleteAsync(id, ct);
        return NoContent();
    }
}
