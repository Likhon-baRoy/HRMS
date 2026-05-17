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

    [Authorize]
    [HttpGet("profile")]
    public async Task<ActionResult<ApiResponse<UserProfileDto>>> Profile()
    {
        var profile = await service.GetProfileAsync();

        return Success(profile);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("users")]
    public async Task<ActionResult<ApiResponse<List<UserAccountDto>>>> Users()
    {
        var users = await service.GetUsersAsync();

        return Success(users);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("users/{id:int}/role")]
    public async Task<ActionResult<ApiResponse<object>>> UpdateRole(int id, UpdateUserRoleDto dto)
    {
        await service.UpdateRoleAsync(id, dto);

        return Success("Role updated successfully");
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("users/{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteUser(int id)
    {
        await service.DeleteUserAsync(id);

        return Success("User account deleted successfully");
    }
}
