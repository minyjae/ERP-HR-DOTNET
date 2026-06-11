using ERP.Application.DTOs;
using ERP.Application.Interfaces;
using ERP.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers;

/// <summary>ใบลา (workflow อนุมัติ) — controller "บาง" error จัดการรวมที่ GlobalExceptionHandler</summary>
[ApiController]
[Route("api/[controller]")]
public class LeaveRequestsController(ILeaveRequestService service) : ControllerBase
{
    /// <summary>ดึงใบลาของพนักงาน (กรองด้วย ?status=Pending &amp; ?year=2026)</summary>
    [HttpGet]
    [ProducesResponseType<List<LeaveRequestResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<LeaveRequestResponse>>> GetByEmployee(
        [FromQuery] Guid employeeId, [FromQuery] LeaveRequestStatus? status, [FromQuery] int? year, CancellationToken ct)
    {
        var result = await service.GetByEmployeeAsync(employeeId, status, year, ct);
        return Ok(result);
    }

    /// <summary>ดึงใบลาตาม id</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<LeaveRequestResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LeaveRequestResponse>> GetById(Guid id, CancellationToken ct)
    {
        var result = await service.GetByIdAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>ยื่นใบลาใหม่ (ระบบคำนวณจำนวนวันลาให้)</summary>
    [HttpPost]
    [ProducesResponseType<LeaveRequestResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<LeaveRequestResponse>> Create(CreateLeaveRequestRequest request, CancellationToken ct)
    {
        var created = await service.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>อนุมัติใบลา (หักโควต้าให้อัตโนมัติ)</summary>
    [HttpPost("{id:guid}/approve")]
    [ProducesResponseType<LeaveRequestResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<LeaveRequestResponse>> Approve(Guid id, ApproveLeaveRequestRequest request, CancellationToken ct)
    {
        var result = await service.ApproveAsync(id, request, ct);
        return Ok(result);
    }

    /// <summary>ปฏิเสธใบลา</summary>
    [HttpPost("{id:guid}/reject")]
    [ProducesResponseType<LeaveRequestResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<LeaveRequestResponse>> Reject(Guid id, RejectLeaveRequestRequest request, CancellationToken ct)
    {
        var result = await service.RejectAsync(id, request, ct);
        return Ok(result);
    }

    /// <summary>ยกเลิกใบลา (ถ้าเคยอนุมัติ จะคืนโควต้าให้อัตโนมัติ)</summary>
    [HttpPost("{id:guid}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken ct)
    {
        await service.CancelAsync(id, ct);
        return NoContent();
    }
}
