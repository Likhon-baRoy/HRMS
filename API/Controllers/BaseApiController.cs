using API.Responses;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    protected ActionResult<ApiResponse<T>> Success<T>(T data, string message = "")
    {
        return Ok(new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        });
    }

    // For action responses
    protected ActionResult<ApiResponse<object>> Success(string message)
    {
        return Ok(
            new ApiResponse<object>
            {
                Success = true,
                Message = message,
                Data = null
            });
    }

    protected ActionResult<ApiResponse<T>> Fail<T>(string message)
    {
        return BadRequest(new ApiResponse<T>
        {
            Success = false,
            Message = message
        });
    }
}
