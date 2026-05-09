using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterUserDto
{
    [Required]
    public int EmployeeId { get; set; }

    [Required]
    public string? Username { get; set; }

    [Required]
    public string? Password { get; set; }

    public string Role { get; set; } = "Employee";
}
