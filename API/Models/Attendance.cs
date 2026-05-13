using API.Models.Enums;

namespace API.Models;

public class Attendance : BaseTrackableEntity
{
    public int EmployeeId { get; set; }

    public Employee Employee { get; set; } = null!;

    public DateOnly Date { get; set; }

    public DateTime CheckInTime { get; set; }

    public DateTime? CheckOutTime { get; set; }

    public AttendanceStatus Status { get; set; } = AttendanceStatus.Present;

    public string? Remarks { get; set; }
}
