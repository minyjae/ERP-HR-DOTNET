using ERP.Application.DTOs;
using FluentValidation;

namespace ERP.Application.Validators;

public class ApproveLeaveRequestRequestValidator : AbstractValidator<ApproveLeaveRequestRequest>
{
    public ApproveLeaveRequestRequestValidator()
    {
        RuleFor(x => x.ApproverId).NotEmpty().WithMessage("กรุณาระบุผู้อนุมัติ");
    }
}
