using ERP.Application.DTOs;
using FluentValidation;

namespace ERP.Application.Validators;

/// <summary>
/// กฎ validation ของการสร้างพนักงาน
/// เน้นตรวจ "รูปแบบ/ความครบถ้วน" ของข้อมูล ส่วนกฎที่ต้องดู database (ค่าซ้ำ) อยู่ใน service
/// </summary>
public class CreateEmployeeRequestValidator : AbstractValidator<CreateEmployeeRequest>
{
    public CreateEmployeeRequestValidator()
    {
        RuleFor(x => x.EmployeeCode)
            .NotEmpty().WithMessage("กรุณาระบุรหัสพนักงาน")
            .MaximumLength(20);

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

        RuleFor(x => x.NationalId)
            .NotEmpty().WithMessage("กรุณาระบุเลขบัตรประชาชน")
            .Length(13).WithMessage("เลขบัตรประชาชนต้องมี 13 หลัก")
            .Matches(@"^\d{13}$").WithMessage("เลขบัตรประชาชนต้องเป็นตัวเลข 13 หลัก");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20)
            .Matches(@"^[0-9+\-\s]+$").When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
            .WithMessage("เบอร์โทรไม่ถูกต้อง");

        RuleFor(x => x.Gender).IsInEnum().WithMessage("เพศไม่ถูกต้อง");

        RuleFor(x => x.DateOfBirth)
            .LessThan(_ => DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("วันเกิดต้องเป็นอดีต");

        RuleFor(x => x.HireDate)
            .NotEqual(default(DateOnly)).WithMessage("กรุณาระบุวันเริ่มงาน");

        RuleFor(x => x.DepartmentId).NotEmpty().WithMessage("กรุณาระบุแผนก");
        RuleFor(x => x.PositionId).NotEmpty().WithMessage("กรุณาระบุตำแหน่ง");
        RuleFor(x => x.BranchId).NotEmpty().WithMessage("กรุณาระบุสาขา");
    }
}
