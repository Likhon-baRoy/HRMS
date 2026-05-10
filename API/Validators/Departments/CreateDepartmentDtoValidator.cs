using API.Data;
using API.DTOs.Departments;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Validators.Departments;

public class CreateDepartmentDtoValidator : AbstractValidator<CreateDepartmentDto>
{
    public CreateDepartmentDtoValidator(AppDbContext context)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100)
            .MustAsync(async (
                name,
                cancellation) =>
            {
                return !await context
                    .Departments
                    .AnyAsync(x =>
                            x.Name == name,
                        cancellation);
            })
            .WithMessage("Department already exists");

        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}
