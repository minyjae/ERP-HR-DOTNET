using ERP.Application.DTOs;
using FluentValidation;

namespace ERP.Application.Validators;

/// <summary>
/// กฎ validation ของการสร้างสาขา
/// เน้นตรวจ "รูปแบบ/ความครบถ้วน" ส่วนกฎที่ต้องดู database (Code ซ้ำ) อยู่ใน service
/// </summary>
public class CreateBranchRequestValidator : AbstractValidator<CreateBranchRequest>
{
    public CreateBranchRequestValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("กรุณาระบุรหัสสาขา")
            .MaximumLength(20);

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
