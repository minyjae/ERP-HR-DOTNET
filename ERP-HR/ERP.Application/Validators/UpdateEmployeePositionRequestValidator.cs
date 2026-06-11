using ERP.Application.DTOs;
using FluentValidation;

namespace ERP.Application.Validators;

public class UpdateEmployeePositionRequestValidator : AbstractValidator<UpdateEmployeePositionRequest>
{
    public UpdateEmployeePositionRequestValidator()
    {
        RuleFor(x => x.Salary).GreaterThan(0).WithMessage("เงินเดือนต้องมากกว่า 0");
        RuleFor(x => x.Remark).MaximumLength(500);
    }
}
