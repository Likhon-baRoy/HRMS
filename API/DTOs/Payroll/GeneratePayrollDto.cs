namespace API.DTOs.Payroll;

public class GeneratePayrollDto
{
    public int EmployeeId { get; set; }

    public DateTime PayPeriodStart { get; set; }

    public DateTime PayPeriodEnd { get; set; }

    public decimal GrossSalary { get; set; }

    public decimal TotalBonus { get; set; }

    public decimal TotalDeductions { get; set; }

    public decimal TaxAmount { get; set; }
}
