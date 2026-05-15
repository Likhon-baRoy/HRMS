using System.ComponentModel.DataAnnotations;

public enum EmploymentType
{
    [Display(Name = "Permanent")] Permanent = 1,

    [Display(Name = "Contract")] Contract = 2,

    [Display(Name = "Probation")] Probation = 3,

    [Display(Name = "Intern")] Intern = 4,

    [Display(Name = "Part Time")] PartTime = 5
}
