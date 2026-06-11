using ERP.Application.DTOs;
using FluentValidation;

namespace ERP.Application.Validators;

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage("กรุณาระบุรหัสผ่านปัจจุบัน");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("กรุณาระบุรหัสผ่านใหม่")
            .MinimumLength(8).WithMessage("รหัสผ่านต้องยาวอย่างน้อย 8 ตัวอักษร")
            .MaximumLength(100)
            .NotEqual(x => x.CurrentPassword).WithMessage("รหัสผ่านใหม่ต้องไม่ซ้ำกับรหัสเดิม");
    }
}
