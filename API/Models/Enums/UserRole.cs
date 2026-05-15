using System.ComponentModel.DataAnnotations;

namespace API.Models.Enums;

public enum UserRole
{
    [Display(Name = "Admin")] Admin = 1,

    [Display(Name = "HR")] Hr = 2,

    [Display(Name = "Manager")] Manager = 3,

    [Display(Name = "Employee")] Employee = 4
}
