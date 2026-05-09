using API.DTOs;
using API.Responses;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AuthController(IAuthService service) : BaseApiController
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login(LoginDto dto)
    {
        var result = await service.LoginAsync(dto);

        return Success(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterUserDto dto)
    {
        await service.RegisterAsync(dto);

        return NoContent();
    }

    [Authorize]
    [HttpGet("me")]
    public ActionResult<ApiResponse<CurrentUserDto>> Me()
    {
        var user = service.GetCurrentUser();

        return Success(user);
    }
}
