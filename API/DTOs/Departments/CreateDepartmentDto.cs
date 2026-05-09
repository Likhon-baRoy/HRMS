using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Departments;

public class CreateDepartmentDto
{
    [Required]
    [MaxLength(100)]
    public string? Name { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public int? ManagerId { get; set; }
}
