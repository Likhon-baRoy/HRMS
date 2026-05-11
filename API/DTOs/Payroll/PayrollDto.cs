using API.Models;

namespace API.DTOs.Payroll;

public class PayrollDto
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public string EmployeeName { get; set; } = string.Empty;

    public DateTime PayPeriodStart { get; set; }

    public DateTime PayPeriodEnd { get; set; }

    public decimal GrossSalary { get; set; }

    public decimal TotalBonus { get; set; }

    public decimal TotalDeductions { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal NetSalary { get; set; }

    public PayrollStatus Status { get; set; }

    public DateTime GeneratedAt { get; set; }
}
