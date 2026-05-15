namespace API.DTOs.Attendance;

public class AttendanceDto
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public string EmployeeName { get; set; } = string.Empty;

    public DateOnly Date { get; set; }

    public DateTime CheckInTime { get; set; }

    public DateTime? CheckOutTime { get; set; }

    public int StatusId { get; set; }

    public string Status { get; set; } = string.Empty;

    public string? Remarks { get; set; }
}
