namespace API.DTOs;

public class UserAccountDto
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public string EmployeeName { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public int RoleId { get; set; }

    public string Role { get; set; } = string.Empty;

    public bool IsProtected { get; set; }
}
