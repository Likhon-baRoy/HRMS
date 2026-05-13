namespace API.Models;

public class Position : BaseEntity
{
    public string Title { get; set; } = string.Empty;

    public string JobLevel { get; set; } = string.Empty;

    public int DepartmentId { get; set; }

    public Department Department { get; set; } = null!;

    public ICollection<Employee> Employees { get; set; } = [];
}
