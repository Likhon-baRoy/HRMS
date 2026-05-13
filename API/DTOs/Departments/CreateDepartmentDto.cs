namespace API.DTOs.Departments;

public class CreateDepartmentDto
{
    public string? Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public int? ManagerId { get; set; }
}
