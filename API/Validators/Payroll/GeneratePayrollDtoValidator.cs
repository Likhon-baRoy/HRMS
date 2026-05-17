using API.Data;
using API.DTOs.Payroll;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Validators.Payroll;

public class GeneratePayrollDtoValidator : AbstractValidator<GeneratePayrollDto>
{
    public GeneratePayrollDtoValidator()
    {
        RuleFor(x => x.PayPeriodStart)
            .LessThan(x => x.PayPeriodEnd)
            .WithMessage("Invalid pay period");

        RuleFor(x => x.PayPeriodStart)
            .NotEmpty();

        RuleFor(x => x.PayPeriodEnd)
            .NotEmpty();

        RuleFor(x => x.BonusAmount)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.DeductionAmount)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.TaxPercent)
            .InclusiveBetween(0, 100);
    }
}
