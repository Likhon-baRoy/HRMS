using API.Data;
using API.DTOs;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Validators.Auth;

public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserDtoValidator(AppDbContext context)
    {
        RuleFor(x => x.EmployeeId)
            .GreaterThan(0)
            .MustAsync(async (
                employeeId,
                cancellation) =>
            {
                return await context
                    .Employees
                    .AnyAsync(x =>
                        x.Id == employeeId,
                        cancellation);
            })
            .WithMessage(
                "Employee not found");

        RuleFor(x => x.Username)
            .NotEmpty()
            .MinimumLength(4)
            .MaximumLength(50)
            .MustAsync(async (
                username,
                cancellation) =>
            {
                return !await context
                    .UserAccounts
                    .AnyAsync(x =>
                        x.Username ==
                        username,
                        cancellation);
            })
            .WithMessage(
                "Username already exists");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Matches(@"[A-Z]")
            .WithMessage(
                "Password must contain at least one uppercase letter")
            .Matches(@"[a-z]")
            .WithMessage(
                "Password must contain at least one lowercase letter")
            .Matches(@"[0-9]")
            .WithMessage(
                "Password must contain at least one number");

        RuleFor(x => x.Role)
            .NotEmpty()
            .Must(role =>
                new[]
                {
                    "Admin",
                    "HR",
                    "Employee"
                }
                .Contains(role))
            .WithMessage(
                "Invalid role");
    }
}
