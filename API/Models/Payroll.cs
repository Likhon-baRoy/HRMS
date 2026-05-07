namespace API.Models;

public class Payroll : BaseEntity
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public Employee Employee { get; set; } = null!;

    public DateTime PayPeriodStart { get; set; }

    public DateTime PayPeriodEnd { get; set; }

    public decimal GrossSalary { get; set; }

    public decimal TotalBonus { get; set; }

    public decimal TotalDeductions { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal NetSalary { get; set; }

    public DateTime GeneratedAt { get; set; }

    public PayrollStatus Status { get; set; }

    public ICollection<PayrollDeduction> PayrollDeductions { get; set; } = [];

    public ICollection<Bonus> Bonuses { get; set; } = [];
}