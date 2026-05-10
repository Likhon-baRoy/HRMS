using API.DTOs.Positions;
using FluentValidation;

namespace API.Validators.Positions;

public class UpdatePositionDtoValidator : AbstractValidator<UpdatePositionDto>
{
    public UpdatePositionDtoValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(100)
            .When(x =>
                !string.IsNullOrWhiteSpace(
                    x.Title));

        RuleFor(x => x.JobLevel)
            .MaximumLength(50)
            .When(x =>
                !string.IsNullOrWhiteSpace(
                    x.JobLevel));

        RuleFor(x => x.DepartmentId)
            .GreaterThan(0)
            .When(x =>
                x.DepartmentId.HasValue);
    }
}
