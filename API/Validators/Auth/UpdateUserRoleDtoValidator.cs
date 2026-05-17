using API.DTOs;
using FluentValidation;

namespace API.Validators.Auth;

public class UpdateUserRoleDtoValidator : AbstractValidator<UpdateUserRoleDto>
{
    public UpdateUserRoleDtoValidator()
    {
        RuleFor(x => x.Role)
            .IsInEnum()
            .WithMessage("Invalid role");
    }
}
