using API.Models.Enums;

namespace API.DTOs;

public class RegisterUserDto
{
    public int EmployeeId { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public UserRole Role { get; set; }
}
