using API.Data;
using API.DTOs.Positions;
using API.Models.Enums;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Validators.Positions;

public class CreatePositionDtoValidator : AbstractValidator<CreatePositionDto>
{
    public CreatePositionDtoValidator(AppDbContext context)
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.JobLevel)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.DepartmentId)
            .GreaterThan(0)
            .MustAsync(async (departmentId, cancellation) =>
            {
                return await context.Departments.AnyAsync(x =>
                        x.Id == departmentId &&
                        x.RecordStatus == RecordStatus.Active,
                    cancellation);
            })
            .WithMessage("Department not found");

        RuleFor(x => x)
            .MustAsync(async (dto, cancellation) =>
            {
                return !await context.Positions.AnyAsync(x =>
                        x.Title == dto.Title &&
                        x.DepartmentId == dto.DepartmentId &&
                        x.RecordStatus != RecordStatus.Inactive,
                    cancellation);
            })
            .WithMessage("Position already exists in this department");
    }
}
