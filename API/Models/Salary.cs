using API.Models;

public class Salary : BaseTrackableEntity
{
    public int EmployeeId { get; set; }

    public Employee Employee { get; set; } = null!;

    public decimal BasicSalary { get; set; }

    public decimal HouseRent { get; set; }

    public decimal MedicalAllowance { get; set; }

    public decimal TransportAllowance { get; set; }

    public decimal OtherAllowance { get; set; }

    public decimal GrossSalary { get; set; }

    public DateTime EffectiveDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool IsCurrent { get; set; } = true;
}