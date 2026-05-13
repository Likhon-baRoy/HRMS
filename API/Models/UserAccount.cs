using API.Models.Enums;

namespace API.Models;

public class UserAccount : BaseEntity
{
    public int EmployeeId { get; set; }

    public Employee Employee { get; set; } = null!;

    public string Username { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.Employee;

    public DateTime? LastLoginAt { get; set; }
}
