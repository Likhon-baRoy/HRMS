using System.ComponentModel.DataAnnotations;

namespace API.Models.Enums;

public enum UserRole
{
    Admin = 1,

    [Display(Name = "HR")]
    Hr = 2,

    Manager = 3,

    Employee = 4
}
