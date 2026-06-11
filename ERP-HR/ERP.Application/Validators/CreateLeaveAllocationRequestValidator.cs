using ERP.Application.DTOs;
using FluentValidation;

namespace ERP.Application.Validators;

/// <summary>กฎ validation ของการสร้างโควต้าวันลา (กฎโควต้าซ้ำที่ต้องดู database อยู่ใน service)</summary>
public class CreateLeaveAllocationRequestValidator : AbstractValidator<CreateLeaveAllocationRequest>
{
    public CreateLeaveAllocationRequestValidator()
    {
        RuleFor(x => x.EmployeeId).NotEmpty().WithMessage("กรุณาระบุพนักงาน");
        RuleFor(x => x.LeaveTypeId).NotEmpty().WithMessage("กรุณาระบุประเภทลา");
        RuleFor(x => x.Year).InclusiveBetween(2000, 2100).WithMessage("ปีไม่ถูกต้อง");
        RuleFor(x => x.TotalDays).GreaterThanOrEqualTo(0).WithMessage("จำนวนวันต้องไม่ติดลบ");
        RuleFor(x => x.CarryOverDays).GreaterThanOrEqualTo(0).WithMessage("วันยกยอดต้องไม่ติดลบ");
    }
}
