using API.Responses;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;

namespace API.Validators;

public class ValidationResultFactory : IFluentValidationAutoValidationResultFactory
{
    public Task<IActionResult?> CreateActionResult(ActionExecutingContext context, ValidationProblemDetails validationProblemDetails, IDictionary<IValidationContext, ValidationResult> validationResults)
    {
        var errors = validationResults
            .SelectMany(x => x.Value.Errors)
            .GroupBy(x =>
                string.IsNullOrEmpty(x.PropertyName)
                    ? x.PropertyName
                    : char.ToLowerInvariant(x.PropertyName[0]) + x.PropertyName[1..]
            )
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.ErrorMessage).ToArray()
            );

        return Task.FromResult<IActionResult?>(
            new BadRequestObjectResult(
                ApiResponse<object>.Fail("Validation failed", errors)
            )
        );
    }
}
