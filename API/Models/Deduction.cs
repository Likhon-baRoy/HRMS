namespace API.Models;

public class Deduction : BaseEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public bool IsPercentage { get; set; }

    public ICollection<PayrollDeduction> PayrollDeductions { get; set; } = [];
}