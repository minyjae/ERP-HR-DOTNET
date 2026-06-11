using ERP.Application.DTOs;
using ERP.Application.Interfaces;
using ERP.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers;

/// <summary>
/// ชั้นบนสุด: รับ HTTP request → เรียก service → คืน HTTP response
/// controller "บาง" — ไม่มี business logic และไม่มี try-catch (error จัดการรวมที่ GlobalExceptionHandler)
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService service) : ControllerBase
{
    /// <summary>ดึงผู้ใช้ทั้งหมด หรือกรองตามสิทธิ์ด้วย ?role=Manager</summary>
    [HttpGet]
    [ProducesResponseType<List<UserResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<UserResponse>>> GetAll([FromQuery] UserRole? role, CancellationToken ct)
    {
        var result = role is null
            ? await service.GetAllAsync(ct)
            : await service.GetByRoleAsync(role.Value, ct);
        return Ok(result);
    }

    /// <summary>ดึงผู้ใช้ตาม id</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<UserResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserResponse>> GetById(Guid id, CancellationToken ct)
    {
        var result = await service.GetByIdAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>สร้างบัญชีผู้ใช้ใหม่ (รับรหัสผ่าน plain แล้ว hash ในระบบ)</summary>
    [HttpPost]
    [ProducesResponseType<UserResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<UserResponse>> Create(CreateUserRequest request, CancellationToken ct)
    {
        var created = await service.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>แก้ไขอีเมล/สิทธิ์ของผู้ใช้</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType<UserResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<UserResponse>> Update(Guid id, UpdateUserRequest request, CancellationToken ct)
    {
        var updated = await service.UpdateAsync(id, request, ct);
        return Ok(updated);
    }

    /// <summary>เปลี่ยนรหัสผ่าน (ต้องยืนยันรหัสผ่านปัจจุบัน)</summary>
    [HttpPut("{id:guid}/password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangePassword(Guid id, ChangePasswordRequest request, CancellationToken ct)
    {
        await service.ChangePasswordAsync(id, request, ct);
        return NoContent();
    }

    /// <summary>เปิดใช้งานบัญชี</summary>
    [HttpPatch("{id:guid}/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Activate(Guid id, CancellationToken ct)
    {
        await service.ActivateAsync(id, ct);
        return NoContent();
    }

    /// <summary>ปิดใช้งานบัญชี (soft — ไม่ลบข้อมูลจริง)</summary>
    [HttpPatch("{id:guid}/deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct)
    {
        await service.DeactivateAsync(id, ct);
        return NoContent();
    }

    /// <summary>ลบบัญชีผู้ใช้</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await service.DeleteAsync(id, ct);
        return NoContent();
    }
}
