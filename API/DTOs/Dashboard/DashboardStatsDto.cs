namespace API.DTOs.Dashboard;

public class DashboardStatsDto
{
    public int TotalEmployees { get; set; }

    public int TotalDepartments { get; set; }

    public int TotalPositions { get; set; }

    public int TodayAttendance { get; set; }

    public int CheckedInToday { get; set; }

    public int CheckedOutToday { get; set; }
}
