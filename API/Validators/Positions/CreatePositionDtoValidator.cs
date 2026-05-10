using API.Data;
using API.DTOs.Positions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Validators.Positions;

public class CreatePositionDtoValidator : AbstractValidator<CreatePositionDto>
{
    public CreatePositionDtoValidator(AppDbContext context)
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(100)
            .MustAsync(async (
                dto,
                title,
                cancellation) =>
            {
                return !await context
                    .Positions
                    .AnyAsync(x =>
                            x.Title == title
                            && x.DepartmentId
                            == dto.DepartmentId
                            && !x.IsDeleted,
                        cancellation);
            })
            .WithMessage(
                "Position already exists in this department");

        RuleFor(x => x.JobLevel)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.DepartmentId)
            .GreaterThan(0)
            .MustAsync(async (
                departmentId,
                cancellation) =>
            {
                return await context
                    .Departments
                    .AnyAsync(x =>
                            x.Id == departmentId,
                        cancellation);
            })
            .WithMessage(
                "Department not found");
    }
}
