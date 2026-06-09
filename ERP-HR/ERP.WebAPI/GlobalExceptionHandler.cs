using ERP.Application.Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api;

/// <summary>
/// จุดเดียวที่แปลง exception จากชั้นล่างให้เป็น HTTP response มาตรฐาน (ProblemDetails)
/// → controller/service ไม่ต้องมี try-catch เต็มไปหมด แค่ "โยน" exception ที่สื่อความหมาย
///   ValidationException → 400, NotFoundException → 404, ConflictException → 409, อื่น ๆ → 500
/// </summary>
public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (status, title) = exception switch
        {
            ValidationException => (StatusCodes.Status400BadRequest, "ข้อมูลไม่ถูกต้อง"),
            NotFoundException => (StatusCodes.Status404NotFound, "ไม่พบข้อมูล"),
            ConflictException => (StatusCodes.Status409Conflict, "ข้อมูลขัดแย้ง"),
            _ => (StatusCodes.Status500InternalServerError, "เกิดข้อผิดพลาดภายในระบบ")
        };

        if (status == StatusCodes.Status500InternalServerError)
            logger.LogError(exception, "Unhandled exception");

        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = exception is ValidationException ? null : exception.Message
        };

        // ถ้าเป็น ValidationException ใส่รายละเอียดราย field ลงไปด้วย
        if (exception is ValidationException vex)
        {
            problem.Extensions["errors"] = vex.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
        }

        httpContext.Response.StatusCode = status;
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
        return true; // จัดการแล้ว ไม่ต้องส่งต่อ
    }
}
