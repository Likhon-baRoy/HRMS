namespace API.Models;

public class Department : BaseEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int? ManagerId { get; set; }

    public Employee? Manager { get; set; }

    public ICollection<Position> Positions { get; set; } = [];

    public ICollection<Employee> Employees { get; set; } = [];
}