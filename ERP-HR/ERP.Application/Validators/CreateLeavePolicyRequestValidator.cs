using ERP.Application.DTOs;
using FluentValidation;

namespace ERP.Application.Validators;

/// <summary>กฎ validation ของการสร้างนโยบายการลา (กฎ active ซ้อนที่ต้องดู database อยู่ใน service)</summary>
public class CreateLeavePolicyRequestValidator : AbstractValidator<CreateLeavePolicyRequest>
{
    public CreateLeavePolicyRequestValidator()
    {
        RuleFor(x => x.LeaveTypeId).NotEmpty().WithMessage("กรุณาระบุประเภทลา");
        RuleFor(x => x.EntitledDays).GreaterThanOrEqualTo(0).WithMessage("สิทธิ์วันลาต้องไม่ติดลบ");
        RuleFor(x => x.MaxCarryOverDays).GreaterThanOrEqualTo(0).WithMessage("วันยกยอดต้องไม่ติดลบ");
        RuleFor(x => x.MinServiceMonths).GreaterThanOrEqualTo(0).WithMessage("อายุงานขั้นต่ำต้องไม่ติดลบ");
        RuleFor(x => x.AdvanceNoticeDays).GreaterThanOrEqualTo(0).WithMessage("วันแจ้งล่วงหน้าต้องไม่ติดลบ");
    }
}
