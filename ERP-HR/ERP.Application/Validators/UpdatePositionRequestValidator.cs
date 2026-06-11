using ERP.Application.DTOs;
using FluentValidation;

namespace ERP.Application.Validators;

public class UpdatePositionRequestValidator : AbstractValidator<UpdatePositionRequest>
{
    public UpdatePositionRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("กรุณาระบุชื่อตำแหน่ง")
            .MaximumLength(150);

        RuleFor(x => x.NameEn).MaximumLength(150);

        RuleFor(x => x.Level)
            .IsInEnum().WithMessage("ระดับตำแหน่งไม่ถูกต้อง");
    }
}
