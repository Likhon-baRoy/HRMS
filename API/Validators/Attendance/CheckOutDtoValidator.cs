using API.Data;
using API.DTOs.Attendance;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Validators.Attendance;

public class CheckOutDtoValidator : AbstractValidator<CheckOutDto>
{
    public CheckOutDtoValidator(AppDbContext context)
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
