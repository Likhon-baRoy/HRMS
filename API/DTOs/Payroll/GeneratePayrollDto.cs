namespace API.DTOs.Payroll;

public class GeneratePayrollDto
{
    public DateTime PayPeriodStart { get; set; }
    public DateTime PayPeriodEnd { get; set; }
    public decimal BonusAmount { get; set; }
    public decimal DeductionAmount { get; set; }
    public decimal TaxPercent { get; set; }
}
