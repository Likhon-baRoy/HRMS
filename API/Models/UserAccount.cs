namespace API.Models;

public class UserAccount : BaseEntity
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public Employee Employee { get; set; } = null!;

    public string Username { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Role { get; set; } = "Employee";

    public bool IsActive { get; set; } = true;

    public DateTime? LastLoginAt { get; set; }
}