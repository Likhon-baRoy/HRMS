namespace API.Models;

public class Employee : BaseEntity
{
    public int Id { get; set; }

    public string EmployeeCode { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public DateTime HireDate { get; set; }

    public EmploymentStatus EmploymentStatus { get; set; }

    public string AccountNumber { get; set; } = string.Empty;

    public int DepartmentId { get; set; }

    public Department Department { get; set; } = null!;

    public int PositionId { get; set; }

    public Position Position { get; set; } = null!;

    public UserAccount? UserAccount { get; set; }

    public ICollection<Salary> Salaries { get; set; } = [];

    public ICollection<Payroll> Payrolls { get; set; } = [];

    public ICollection<Attendance> Attendances { get; set; } = [];
}
