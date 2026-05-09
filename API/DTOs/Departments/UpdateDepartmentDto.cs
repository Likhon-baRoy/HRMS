using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Departments;

public class UpdateDepartmentDto
{
    [MaxLength(100)]
    public string? Name { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public int? ManagerId { get; set; }
}
