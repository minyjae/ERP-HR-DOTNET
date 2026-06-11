using ERP.Application.DTOs;
using FluentValidation;

namespace ERP.Application.Validators;

/// <summary>กฎ validation ของการสร้างประเภทลา (กฎ Code ซ้ำที่ต้องดู database อยู่ใน service)</summary>
public class CreateLeaveTypeRequestValidator : AbstractValidator<CreateLeaveTypeRequest>
{
    public CreateLeaveTypeRequestValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("กรุณาระบุรหัสประเภทลา")
            .MaximumLength(20);

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("กรุณาระบุชื่อประเภทลา")
            .MaximumLength(150);
    }
}
