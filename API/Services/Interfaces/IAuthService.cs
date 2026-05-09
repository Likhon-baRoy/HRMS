using API.DTOs;

namespace API.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginDto dto);

    Task RegisterAsync(RegisterUserDto dto);

    CurrentUserDto GetCurrentUser();
}
