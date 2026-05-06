using System.Net;
using API.Exceptions;
using API.Responses;

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
                ApiResponse<object>.Fail(ex.Message, ex.Errors )
            );
        }
        catch (NotFoundException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;

            await context.Response.WriteAsJsonAsync(
                ApiResponse<object>.Fail(ex.Message)
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
