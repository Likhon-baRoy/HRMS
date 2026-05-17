using API.Data;
using API.DTOs.Salary;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Validators.Salary;

public class UpdateSalaryDtoValidator : AbstractValidator<UpdateSalaryDto>
{
  public UpdateSalaryDtoValidator(AppDbContext context)
  {
    RuleFor(x => x.EmployeeId)
        .MustAsync(async (
            id,
            cancellation
        ) =>
        {
          return await context
                  .Employees
                  .AnyAsync(
                      x => x.Id == id,
                      cancellation
                  );
        })
        .WithMessage("Employee not found");

    RuleFor(x => x.BasicSalary)
        .GreaterThan(0);

    RuleFor(x => x.HouseRent)
        .GreaterThanOrEqualTo(0);

    RuleFor(x => x.MedicalAllowance)
        .GreaterThanOrEqualTo(0);

    RuleFor(x => x.TransportAllowance)
        .GreaterThanOrEqualTo(0);

    RuleFor(x => x.OtherAllowance)
        .GreaterThanOrEqualTo(0);

    RuleFor(x => x.EffectiveDate)
        .NotEmpty();
  }
}
