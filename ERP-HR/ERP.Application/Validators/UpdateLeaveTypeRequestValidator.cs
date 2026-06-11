using ERP.Application.DTOs;
using FluentValidation;

namespace ERP.Application.Validators;

public class UpdateLeaveTypeRequestValidator : AbstractValidator<UpdateLeaveTypeRequest>
{
    public UpdateLeaveTypeRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("กรุณาระบุชื่อประเภทลา")
            .MaximumLength(150);
    }
}
