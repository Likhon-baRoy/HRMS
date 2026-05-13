namespace API.Models;

public class Bonus : BaseTrackableEntity
{
    public int PayrollId { get; set; }

    public Payroll Payroll { get; set; } = null!;

    public string BonusType { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public string Remarks { get; set; } = string.Empty;
}
