using System.Net;
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
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            await context.Response.WriteAsJsonAsync(
                ApiResponse<object>.Fail(ex.Message, ex.Errors)
            );
        }
        catch (NotFoundException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;

            await context.Response.WriteAsJsonAsync(
                ApiResponse<object>.Fail(ex.Message)
            );
        }
        catch (UnauthorizedAccessException ex)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            await context.Response.WriteAsJsonAsync(
                ApiResponse<object>.Fail(ex.Message)
            );
        }
        catch (AccountDisabledException ex)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;

            await context.Response.WriteAsJsonAsync(
                ApiResponse<object>.Fail(ex.Message)
            );
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, ex.Message);

            var dbMessage = ex.InnerException?.Message ?? ex.Message;

            var errors = new Dictionary<string, string[]>();

            // Duplicate value checks
            if (dbMessage.Contains("email", StringComparison.OrdinalIgnoreCase))
            {
                errors["email"] =
                [
                    "Email already exists"
                ];
            }
            else if (dbMessage.Contains("phone", StringComparison.OrdinalIgnoreCase))
            {
                errors["phone"] =
                [
                    "Phone already exists"
                ];
            }
            else if (dbMessage.Contains("employeecode", StringComparison.OrdinalIgnoreCase))
            {
                errors["employeeCode"] =
                [
                    "Employee code already exists"
                ];
            }
            // Required / nullable column issue
            else if (dbMessage.Contains("cannot be null", StringComparison.OrdinalIgnoreCase))
            {
                errors["database"] =
                [
                    env.IsDevelopment()
                        ? dbMessage
                        : "Required field missing"
                ];
            }
            else
            {
                errors["database"] =
                [
                    env.IsDevelopment()
                        ? dbMessage
                        : "Database operation failed"
                ];
            }

            context.Response.StatusCode = StatusCodes.Status400BadRequest;

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

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

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
