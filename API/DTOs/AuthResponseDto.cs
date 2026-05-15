namespace API.DTOs;

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public int RoleId { get; set; }

    public string Role { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }
}
