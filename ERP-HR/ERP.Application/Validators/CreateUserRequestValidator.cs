using ERP.Application.DTOs;
using FluentValidation;

namespace ERP.Application.Validators;

/// <summary>
/// กฎ validation ของการสร้างบัญชีผู้ใช้
/// เน้นตรวจ "รูปแบบ/ความครบถ้วน" ส่วนกฎที่ต้องดู database (email/บัญชีซ้ำ) อยู่ใน service
/// </summary>
public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("กรุณาระบุพนักงานเจ้าของบัญชี");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("กรุณาระบุอีเมล")
            .EmailAddress().WithMessage("รูปแบบอีเมลไม่ถูกต้อง")
            .MaximumLength(150);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("กรุณาระบุรหัสผ่าน")
            .MinimumLength(8).WithMessage("รหัสผ่านต้องยาวอย่างน้อย 8 ตัวอักษร")
            .MaximumLength(100);

        RuleFor(x => x.Role)
            .IsInEnum().WithMessage("สิทธิ์ผู้ใช้ไม่ถูกต้อง");
    }
}
