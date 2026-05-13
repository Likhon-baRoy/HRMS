using API.Models.Enums;

namespace API.DTOs;

public class EmployeeDto
{
    public int Id { get; set; }

    public string? EmployeeCode { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public DateTime DateOfBirth { get; set; }

    public DateTime HireDate { get; set; }

    public string EmploymentStatus { get; set; } = string.Empty;

    public string? AccountNumber { get; set; }

    public int DepartmentId { get; set; }

    public string? DepartmentName { get; set; }

    public int PositionId { get; set; }

    public string? PositionTitle { get; set; }

    public string Status { get; set; } = string.Empty;
}
