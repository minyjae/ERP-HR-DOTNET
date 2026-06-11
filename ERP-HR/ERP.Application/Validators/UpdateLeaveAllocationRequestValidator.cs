using ERP.Application.DTOs;
using FluentValidation;

namespace ERP.Application.Validators;

public class UpdateLeaveAllocationRequestValidator : AbstractValidator<UpdateLeaveAllocationRequest>
{
    public UpdateLeaveAllocationRequestValidator()
    {
        RuleFor(x => x.TotalDays).GreaterThanOrEqualTo(0).WithMessage("จำนวนวันต้องไม่ติดลบ");
        RuleFor(x => x.CarryOverDays).GreaterThanOrEqualTo(0).WithMessage("วันยกยอดต้องไม่ติดลบ");
    }
}
