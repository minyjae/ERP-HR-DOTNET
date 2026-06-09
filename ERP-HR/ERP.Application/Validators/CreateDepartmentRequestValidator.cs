using ERP.Application.DTOs;
using FluentValidation;

namespace ERP.Application.Validators;

/// <summary>
/// กฎ validation ของการสร้างแผนก
/// เน้นตรวจ "รูปแบบ/ความครบถ้วน" ส่วนกฎที่ต้องดู database (Code ซ้ำ) อยู่ใน service
/// </summary>
public class CreateDepartmentRequestValidator : AbstractValidator<CreateDepartmentRequest>
{
    public CreateDepartmentRequestValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("กรุณาระบุรหัสแผนก")
            .MaximumLength(20);

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("กรุณาระบุชื่อแผนก")
            .MaximumLength(150);

        RuleFor(x => x.NameEn).MaximumLength(150);
    }
}