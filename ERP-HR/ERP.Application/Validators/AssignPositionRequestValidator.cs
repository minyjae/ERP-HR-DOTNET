using ERP.Application.DTOs;
using FluentValidation;

namespace ERP.Application.Validators;

/// <summary>กฎ validation ของการมอบหมายตำแหน่ง (รูปแบบ/ความครบถ้วน — กฎข้ามตำแหน่งอยู่ใน service)</summary>
public class AssignPositionRequestValidator : AbstractValidator<AssignPositionRequest>
{
    public AssignPositionRequestValidator()
    {
        RuleFor(x => x.EmployeeId).NotEmpty().WithMessage("กรุณาระบุพนักงาน");
        RuleFor(x => x.PositionId).NotEmpty().WithMessage("กรุณาระบุตำแหน่ง");
        RuleFor(x => x.DepartmentId).NotEmpty().WithMessage("กรุณาระบุแผนก");
        RuleFor(x => x.StartDate).NotEmpty().WithMessage("กรุณาระบุวันเริ่ม");
        RuleFor(x => x.Salary).GreaterThan(0).WithMessage("เงินเดือนต้องมากกว่า 0");
        RuleFor(x => x.Remark).MaximumLength(500);
    }
}
