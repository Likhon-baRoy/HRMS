using API.DTOs;

namespace API.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginDto dto);

    Task RegisterAsync(RegisterUserDto dto);

    CurrentUserDto GetCurrentUser();

    Task<UserProfileDto> GetProfileAsync();

    Task<List<UserAccountDto>> GetUsersAsync();

    Task UpdateRoleAsync(int id, UpdateUserRoleDto dto);

    Task DeleteUserAsync(int id);
}
