using API.Data;
using API.DTOs;
using API.Models.Enums;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Validators.Employees;

public class UpdateEmployeeDtoValidator : AbstractValidator<UpdateEmployeeDto>
{
    public UpdateEmployeeDtoValidator(AppDbContext context)
    {
        RuleFor(x => x.EmployeeCode)
            .MaximumLength(20);

        RuleFor(x => x.FirstName)
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .EmailAddress()
            .MaximumLength(150)
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.Phone)
            .MinimumLength(9)
            .MaximumLength(11)
            .When(x => !string.IsNullOrWhiteSpace(x.Phone))
            .MustAsync(async (dto, phone, cancellation) =>
            {
                return !await context.Employees
                    .AnyAsync(x =>
                            x.Id != dto.Id &&
                            x.Phone == phone &&
                            x.EmployeeStatus != EmployeeStatus.Resigned &&
                            x.EmployeeStatus != EmployeeStatus.Terminated,
                        cancellation);
            })
            .WithMessage("Phone already exists");

        RuleFor(x => x.DepartmentId)
            .GreaterThan(0)
            .When(x => x.DepartmentId.HasValue);

        RuleFor(x => x.PositionId)
            .GreaterThan(0)
            .When(x => x.PositionId.HasValue);

        RuleFor(x => x.EmploymentType)
            .IsInEnum()
            .When(x => x.EmploymentType.HasValue);

        RuleFor(x => x.EmployeeStatus)
            .IsInEnum()
            .When(x => x.EmployeeStatus.HasValue);


        RuleFor(x => x.DateOfBirth)
            .Must(date =>
                date <= DateTime.Today.AddYears(-18)
            )
            .WithMessage("Employee must be at least 18 years old");
    }
}
