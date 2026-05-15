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

    public int EmploymentTypeId { get; set; }

    public string EmploymentType { get; set; } = string.Empty;

    public int EmployeeStatusId { get; set; }

    public string EmployeeStatus { get; set; } = string.Empty;

    public string? AccountNumber { get; set; }

    public int DepartmentId { get; set; }

    public string? DepartmentName { get; set; }

    public int PositionId { get; set; }

    public string? PositionTitle { get; set; }

    public int RecordStatusId { get; set; }

    public string RecordStatus { get; set; } = string.Empty;
}
