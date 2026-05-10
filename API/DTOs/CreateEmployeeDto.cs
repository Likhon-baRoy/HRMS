using System.ComponentModel.DataAnnotations;
using API.Models;

namespace API.DTOs;

public class CreateEmployeeDto
{
    [Required] public string? EmployeeCode { get; set; }

    [Required] public string? FirstName { get; set; }

    [Required] public string? LastName { get; set; }

    [Required] [EmailAddress] public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    [Required] public DateTime DateOfBirth { get; set; }

    [Required] public DateTime HireDate { get; set; }

    [Required] public EmploymentStatus EmploymentStatus { get; set; }

    public string? AccountNumber { get; set; }

    [Required] public int DepartmentId { get; set; }

    [Required] public int PositionId { get; set; }
}
