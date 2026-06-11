using ERP.Application.DTOs;
using FluentValidation;

namespace ERP.Application.Validators;

public class RejectLeaveRequestRequestValidator : AbstractValidator<RejectLeaveRequestRequest>
{
    public RejectLeaveRequestRequestValidator()
    {
        RuleFor(x => x.ApproverId).NotEmpty().WithMessage("กรุณาระบุผู้ดำเนินการ");
        RuleFor(x => x.RejectReason)
            .NotEmpty().WithMessage("กรุณาระบุเหตุผลการปฏิเสธ")
            .MaximumLength(500);
    }
}
