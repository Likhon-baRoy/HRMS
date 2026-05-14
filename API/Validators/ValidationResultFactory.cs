using API.Responses;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;

namespace API.Validators;

public class ValidationResultFactory : IFluentValidationAutoValidationResultFactory
{
    public Task<IActionResult?> CreateActionResult(
        ActionExecutingContext context,
        ValidationProblemDetails? validationProblemDetails,
        IDictionary<IValidationContext, ValidationResult> validationResults)
    {
        var errors = new Dictionary<string, string[]>();

        // FluentValidation errors
        foreach (var validationResult in validationResults.Values)
        {
            foreach (var error in validationResult.Errors)
            {
                if (!errors.ContainsKey(error.PropertyName))
                {
                    errors[error.PropertyName] = [];
                }

                errors[error.PropertyName] = errors[error.PropertyName]
                    .Append(error.ErrorMessage)
                    .Distinct()
                    .ToArray();
            }
        }

        // Model binding errors
        foreach (var state in context.ModelState)
        {
            var key = state.Key;

            var messages = state.Value.Errors
                .Select(x => x.ErrorMessage)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToArray();

            if (messages is { Length: > 0 })
            {
                errors[key] = messages;
            }
        }

        return Task.FromResult<IActionResult?>(
            new BadRequestObjectResult(
                ApiResponse<object>.Fail(
                    "Validation failed",
                    errors
                )
            )
        );
    }
}
