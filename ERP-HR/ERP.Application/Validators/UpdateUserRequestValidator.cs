using ERP.Application.DTOs;
using FluentValidation;

namespace ERP.Application.Validators;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("กรุณาระบุอีเมล")
            .EmailAddress().WithMessage("รูปแบบอีเมลไม่ถูกต้อง")
            .MaximumLength(150);

        RuleFor(x => x.Role)
            .IsInEnum().WithMessage("สิทธิ์ผู้ใช้ไม่ถูกต้อง");
    }
}
