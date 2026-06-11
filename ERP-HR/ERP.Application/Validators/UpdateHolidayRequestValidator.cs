using ERP.Application.DTOs;
using FluentValidation;

namespace ERP.Application.Validators;

public class UpdateHolidayRequestValidator : AbstractValidator<UpdateHolidayRequest>
{
    public UpdateHolidayRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("กรุณาระบุชื่อวันหยุด")
            .MaximumLength(150);

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("กรุณาระบุวันที่");
    }
}
