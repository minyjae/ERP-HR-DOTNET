using ERP.Application.DTOs;
using FluentValidation;

namespace ERP.Application.Validators;

public class UpdateBranchRequestValidator : AbstractValidator<UpdateBranchRequest>
{
    public UpdateBranchRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("กรุณาระบุชื่อสาขา")
            .MaximumLength(150);

        RuleFor(x => x.Address).MaximumLength(500);

        RuleFor(x => x.Phone)
            .MaximumLength(20)
            .Matches(@"^[0-9+\-\s]+$").When(x => !string.IsNullOrWhiteSpace(x.Phone))
            .WithMessage("เบอร์โทรไม่ถูกต้อง");
    }
}
