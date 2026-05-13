namespace API.Models;

public class SalaryRevision : BaseTrackableEntity
{
    public int SalaryId { get; set; }

    public Salary Salary { get; set; } = null!;

    public decimal OldSalary { get; set; }

    public decimal NewSalary { get; set; }

    public string Reason { get; set; } = string.Empty;

    public DateTime RevisionDate { get; set; }

    public int ApprovedBy { get; set; }
}
