namespace API.Models;

public class Salary : BaseEntity
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public Employee Employee { get; set; } = null!;

    public decimal BaseSalary { get; set; }

    public decimal Allowance { get; set; }

    public DateTime EffectiveDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool IsCurrent { get; set; }
}