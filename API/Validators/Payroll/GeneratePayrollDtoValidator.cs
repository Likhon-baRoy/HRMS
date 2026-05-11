using API.Data;
using API.DTOs.Payroll;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Validators.Payroll;

public class GeneratePayrollDtoValidator : AbstractValidator<GeneratePayrollDto>
{
    public GeneratePayrollDtoValidator(AppDbContext context)
    {
        RuleFor(x =>
                x.EmployeeId)
            .GreaterThan(0)
            .MustAsync(async (
                id,
                cancellation) =>
            {
                return await context
                    .Employees
                    .AnyAsync(x =>
                            x.Id == id,
                        cancellation);
            })
            .WithMessage("Employee not found");

        RuleFor(x =>
                x.PayPeriodStart)
            .LessThan(x =>
                x.PayPeriodEnd);

        RuleFor(x =>
                x.GrossSalary)
            .GreaterThanOrEqualTo(0);

        RuleFor(x =>
                x.TotalBonus)
            .GreaterThanOrEqualTo(0);

        RuleFor(x =>
                x.TotalDeductions)
            .GreaterThanOrEqualTo(0);

        RuleFor(x =>
                x.TaxAmount)
            .GreaterThanOrEqualTo(0);
    }
}
