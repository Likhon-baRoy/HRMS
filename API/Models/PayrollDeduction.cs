namespace API.Models;

public class PayrollDeduction
{
    public int Id { get; set; }

    public int PayrollId { get; set; }

    public Payroll Payroll { get; set; } = null!;

    public int DeductionId { get; set; }

    public Deduction Deduction { get; set; } = null!;

    public decimal AppliedAmount { get; set; }
}