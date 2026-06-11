using ERP.Application.DTOs;
using FluentValidation;

namespace ERP.Application.Validators;

/// <summary>กฎ validation ของการสร้างวันหยุด (กฎวันซ้ำที่ต้องดู database อยู่ใน service)</summary>
public class CreateHolidayRequestValidator : AbstractValidator<CreateHolidayRequest>
{
    public CreateHolidayRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("กรุณาระบุชื่อวันหยุด")
            .MaximumLength(150);

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("กรุณาระบุวันที่");
    }
}
