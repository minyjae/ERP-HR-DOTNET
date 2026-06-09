using ERP.Application.DTOs;
using FluentValidation;

namespace ERP.Application.Validators;

public class UpdateEmployeeRequestValidator : AbstractValidator<UpdateEmployeeRequest>
{
    public UpdateEmployeeRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("กรุณาระบุชื่อ")
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("กรุณาระบุนามสกุล")
            .MaximumLength(100);

        RuleFor(x => x.FirstNameEn).MaximumLength(100);
        RuleFor(x => x.LastNameEn).MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("กรุณาระบุอีเมล")
            .EmailAddress().WithMessage("รูปแบบอีเมลไม่ถูกต้อง")
            .MaximumLength(256);

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20)
            .Matches(@"^[0-9+\-\s]+$").When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
            .WithMessage("เบอร์โทรไม่ถูกต้อง");

        RuleFor(x => x.Gender).IsInEnum().WithMessage("เพศไม่ถูกต้อง");

        RuleFor(x => x.DepartmentId).NotEmpty().WithMessage("กรุณาระบุแผนก");
        RuleFor(x => x.PositionId).NotEmpty().WithMessage("กรุณาระบุตำแหน่ง");
        RuleFor(x => x.BranchId).NotEmpty().WithMessage("กรุณาระบุสาขา");
    }
}
