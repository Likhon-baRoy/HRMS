using API.Models;

namespace API.DTOs;

public class UpdateEmployeeDto
{
    public string? EmployeeCode { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public DateTime? HireDate { get; set; }

    public EmploymentStatus? EmploymentStatus { get; set; }

    public string? AccountNumber { get; set; }

    public int? DepartmentId { get; set; }

    public int? PositionId { get; set; }
}
