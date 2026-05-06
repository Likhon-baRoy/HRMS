using System.Net;
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
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = ApiResponse<object>.Fail(
                env.IsDevelopment()
                    ? ex.Message
                    : "Something went wrong"
            );

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
