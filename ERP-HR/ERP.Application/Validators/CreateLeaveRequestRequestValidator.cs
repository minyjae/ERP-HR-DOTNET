using ERP.Application.DTOs;
using FluentValidation;

namespace ERP.Application.Validators;

/// <summary>กฎ validation ของการยื่นใบลา (กฎที่ต้องดู database เช่น โควต้า/ทับซ้อน อยู่ใน service)</summary>
public class CreateLeaveRequestRequestValidator : AbstractValidator<CreateLeaveRequestRequest>
{
    public CreateLeaveRequestRequestValidator()
    {
        RuleFor(x => x.EmployeeId).NotEmpty().WithMessage("กรุณาระบุพนักงาน");
        RuleFor(x => x.LeaveTypeId).NotEmpty().WithMessage("กรุณาระบุประเภทลา");
        RuleFor(x => x.StartDate).NotEmpty().WithMessage("กรุณาระบุวันเริ่มลา");
        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("กรุณาระบุวันสิ้นสุด")
            .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("วันสิ้นสุดต้องไม่ก่อนวันเริ่ม");
        RuleFor(x => x.Reason).MaximumLength(500);
        RuleFor(x => x.AttachmentUrl).MaximumLength(500);
    }
}
