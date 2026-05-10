using System.Net;
using System.Reflection.Metadata;
using API.Exceptions;
using API.Responses;
using Microsoft.EntityFrameworkCore;

namespace API.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IWebHostEnvironment env)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (AppValidationException ex)
        {
            context.Response.StatusCode =
                (int)HttpStatusCode.BadRequest;

            await context.Response.WriteAsJsonAsync(
                ApiResponse<object>.Fail(ex.Message, ex.Errors)
            );
        }
        catch (NotFoundException ex)
        {
            context.Response.StatusCode =
                (int)HttpStatusCode.NotFound;

            await context.Response.WriteAsJsonAsync(
                ApiResponse<object>.Fail(ex.Message)
            );
        }
        catch (UnauthorizedAccessException ex)
        {
            context.Response.StatusCode =
                StatusCodes.Status401Unauthorized;

            await context.Response.WriteAsJsonAsync(
                ApiResponse<object>.Fail(ex.Message)
            );
        }
        catch (DbUpdateException ex)
        {
            var message =
                ex.InnerException?.Message ??
                ex.Message;

            var errors =
                new Dictionary<string, string[]>();

            if (message.Contains("email",
                    StringComparison
                        .OrdinalIgnoreCase))
            {
                errors["email"] = [ "Email already exists" ];
            }

            else if (message.Contains(
                         "phone",
                         StringComparison
                             .OrdinalIgnoreCase))
            {
                errors["phone"] = [ "Phone already exists" ];
            }

            else if (message.Contains(
                         "employee_code",
                         StringComparison
                             .OrdinalIgnoreCase))
            {
                errors["employeeCode"] = [ "Employee code already exists" ];
            }

            else
            {
                errors["database"] =
                [
                    message
                ];
            }

            context.Response.StatusCode =
                (int)HttpStatusCode.BadRequest;

            await context.Response.WriteAsJsonAsync(
                ApiResponse<object>.Fail(
                    "Validation failed",
                    errors
                )
            );
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            context.Response.StatusCode =
                (int)HttpStatusCode.InternalServerError;

            await context.Response.WriteAsJsonAsync(
                ApiResponse<object>.Fail(
                    env.IsDevelopment()
                        ? ex.Message
                        : "Something went wrong"
                )
            );
        }
    }
}
