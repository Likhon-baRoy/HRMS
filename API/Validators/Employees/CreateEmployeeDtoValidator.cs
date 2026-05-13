using API.Data;
using API.DTOs;
using API.Models.Enums;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Validators.Employees;

public class CreateEmployeeDtoValidator : AbstractValidator<CreateEmployeeDto>
{
    public CreateEmployeeDtoValidator(AppDbContext context)
    {
        RuleFor(x => x.EmployeeCode)
            .NotEmpty()
            .MaximumLength(20)
            .MustAsync(async (code, cancellation) =>
            {
                return !await context
                    .Employees
                    .AnyAsync(x => x.EmployeeCode == code,
                        cancellation);
            })
            .WithMessage("Employee code already exists");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(150)
            .MustAsync(async (email, cancellation) =>
            {
                return !await context
                    .Employees
                    .AnyAsync(x => x.Email == email && x.RecordStatus == RecordStatus.Active,
                        cancellation);
            })
            .WithMessage("Email already exists");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .MinimumLength(9)
            .MaximumLength(11)
            .MustAsync(async (phone, cancellation) =>
            {
                if (string.IsNullOrWhiteSpace(phone)) return true;

                return !await context.Employees
                    .AnyAsync(x => x.Phone == phone && x.RecordStatus == RecordStatus.Active,
                        cancellation);
            })
            .WithMessage("Phone already exists");

        RuleFor(x => x.DepartmentId)
            .MustAsync(async (id, cancellation) =>
            {
                return await context
                    .Departments
                    .AnyAsync(x => x.Id == id,
                        cancellation);
            })
            .WithMessage("Department not found");

        RuleFor(x => x.PositionId)
            .MustAsync(async (id, cancellation) =>
            {
                return await context
                    .Positions
                    .AnyAsync(x => x.Id == id,
                        cancellation);
            })
            .WithMessage("Position not found");
    }
}
