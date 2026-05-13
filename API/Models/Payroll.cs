using API.Models.Enums;

namespace API.Models;

public class Payroll : BaseTrackableEntity
{
    public int EmployeeId { get; set; }

    public Employee Employee { get; set; } = null!;

    public DateTime PayPeriodStart { get; set; }

    public DateTime PayPeriodEnd { get; set; }

    public decimal GrossSalary { get; set; }

    public decimal TotalBonus { get; set; }

    public decimal TotalDeductions { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal NetSalary { get; set; }

    public void CalculateNetSalary()
    {
        NetSalary =
            GrossSalary
            + TotalBonus
            - TotalDeductions
            - TaxAmount;
    }

    public DateTime GeneratedAt { get; set; }

    public PayrollStatus Status { get; set; } = PayrollStatus.Pending;

    public ICollection<PayrollDeduction> PayrollDeductions { get; set; } = [];

    public ICollection<Bonus> Bonuses { get; set; } = [];
}
