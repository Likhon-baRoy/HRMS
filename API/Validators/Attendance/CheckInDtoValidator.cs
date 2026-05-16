using API.Data;
using API.DTOs.Attendance;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Validators.Attendance;

public class CheckInDtoValidator : AbstractValidator<CheckInDto>
{
    public CheckInDtoValidator()
    {
        RuleFor(x => x.Remarks)
            .MaximumLength(500);
    }
}
