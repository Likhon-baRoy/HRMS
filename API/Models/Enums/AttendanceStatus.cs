using System.ComponentModel.DataAnnotations;

namespace API.Models.Enums;

public enum AttendanceStatus
{
    Present = 1,
    Absent = 2,
    Late = 3,
    [Display(Name = "Half Day")] HalfDay = 4,
    Leave = 5
}
