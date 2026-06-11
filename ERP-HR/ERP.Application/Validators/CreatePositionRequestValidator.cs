using ERP.Application.DTOs;
using FluentValidation;

namespace ERP.Application.Validators;

/// <summary>
/// กฎ validation ของการสร้างตำแหน่ง
/// เน้นตรวจ "รูปแบบ/ความครบถ้วน" ส่วนกฎที่ต้องดู database (Code ซ้ำ) อยู่ใน service
/// </summary>
public class CreatePositionRequestValidator : AbstractValidator<CreatePositionRequest>
{
    public CreatePositionRequestValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("กรุณาระบุรหัสตำแหน่ง")
            .MaximumLength(20);

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("กรุณาระบุชื่อตำแหน่ง")
            .MaximumLength(150);

        RuleFor(x => x.NameEn).MaximumLength(150);

        RuleFor(x => x.Level)
            .IsInEnum().WithMessage("ระดับตำแหน่งไม่ถูกต้อง");
    }
}
