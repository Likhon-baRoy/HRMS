using API.DTOs;
using FluentValidation;

namespace API.Validators.Employees;

public class UpdateEmployeeDtoValidator : AbstractValidator<UpdateEmployeeDto>
{
    public UpdateEmployeeDtoValidator()
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
            .When(x => !string.IsNullOrWhiteSpace(x.Phone));

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
