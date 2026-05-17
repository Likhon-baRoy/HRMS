namespace API.DTOs;

public class UserProfileDto
{
    public int UserId { get; set; }

    public int EmployeeId { get; set; }

    public string Username { get; set; } = string.Empty;

    public int RoleId { get; set; }

    public string Role { get; set; } = string.Empty;

    public string EmployeeCode { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public DateTime HireDate { get; set; }

    public string EmploymentType { get; set; } = string.Empty;

    public string EmployeeStatus { get; set; } = string.Empty;

    public string? AccountNumber { get; set; }

    public string DepartmentName { get; set; } = string.Empty;

    public string PositionTitle { get; set; } = string.Empty;

    public decimal? BasicSalary { get; set; }

    public decimal? GrossSalary { get; set; }

    public DateTime? SalaryEffectiveDate { get; set; }
}
