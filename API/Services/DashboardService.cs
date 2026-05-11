using API.Data;
using API.DTOs.Dashboard;
using API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class DashboardService(AppDbContext context) : IDashboardService
{
    public async Task<DashboardStatsDto> GetStatsAsync()
    {
        var today =
            DateOnly.FromDateTime(
                DateTime.UtcNow);

        return new
            DashboardStatsDto
            {
                TotalEmployees =
                    await context
                        .Employees
                        .CountAsync(),

                TotalDepartments =
                    await context
                        .Departments
                        .CountAsync(),

                TotalPositions =
                    await context
                        .Positions
                        .CountAsync(),

                TodayAttendance =
                    await context
                        .Attendances
                        .CountAsync(x =>
                            x.Date
                            == today),

                CheckedInToday =
                    await context
                        .Attendances
                        .CountAsync(x =>
                            x.Date
                            == today),

                CheckedOutToday =
                    await context
                        .Attendances
                        .CountAsync(x =>
                            x.Date
                            == today
                            && x.CheckOutTime
                            != null)
            };
    }
}
