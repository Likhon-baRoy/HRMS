using API.Data;
using API.DTOs.Attendance;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Validators.Attendance;

public class CheckInDtoValidator : AbstractValidator<CheckInDto>
{
    public CheckInDtoValidator(AppDbContext context)
    {
        RuleFor(x => x.EmployeeId)
            .GreaterThan(0)
            .MustAsync(async (id, cancellation) =>
            {
                return await context
                    .Employees
                    .AnyAsync(x =>
                            x.Id == id,
                        cancellation);
            })
            .WithMessage("Employee not found");
    }
}
