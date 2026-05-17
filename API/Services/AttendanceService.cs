using API.Data;
using API.DTOs.Attendance;
using API.Exceptions;
using API.Extensions;
using API.Models;
using API.Models.Enums;
using API.Requests;
using API.Responses;
using API.Services.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class AttendanceService(AppDbContext context, IMapper mapper, ICurrentUserService currentUser)
    : IAttendanceService
{
    public async Task CheckInAsync(CheckInDto dto)
    {
        var employeeId = currentUser.EmployeeId ?? throw new UnauthorizedAccessException("Unauthorized");

        var employee =
            await context.Employees
                .GetByIdOrThrowAsync(employeeId);

        if (
            employee.EmployeeStatus
            is EmployeeStatus.Terminated
            or EmployeeStatus.Resigned
            or EmployeeStatus.Suspended
        )
        {
            throw new
                AppValidationException(
                    "Validation failed",
                    new Dictionary<
                        string,
                        string[]
                    >
                    {
                        {
                            "attendance",
                            [
                                "Employee is inactive"
                            ]
                        }
                    }
                );
        }

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var exists =
            await context
                .Attendances
                .AnyAsync(x =>
                    x.EmployeeId == employeeId &&
                    x.Date == today);

        if (exists)
        {
            throw new
                AppValidationException(
                    "Validation failed",
                    new Dictionary<string, string[]>
                    {
                        {
                            "attendance",
                            [
                                "Already checked in today"
                            ]
                        }
                    });
        }

        var now = DateTime.UtcNow;

        var status = AttendanceStatus.Present;

        if (now.TimeOfDay > new TimeSpan(9, 30, 0))
        {
            status = AttendanceStatus.Late;
        }

        var attendance = new Attendance
        {
            EmployeeId = employeeId,
            Date = today,
            CheckInTime = now,
            Status = status,
            Remarks = dto.Remarks
        };

        context.Attendances.Add(attendance);

        await context.SaveChangesAsync();
    }

    public async Task CheckOutAsync()
    {
        var employeeId = currentUser.EmployeeId ?? throw new UnauthorizedAccessException("Unauthorized");

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var attendance =
            await context
                .Attendances
                .FirstOrDefaultAsync(x =>
                    x.EmployeeId == employeeId &&
                    x.Date == today);


        if (attendance == null)
        {
            throw new
                AppValidationException(
                    "Validation failed",
                    new Dictionary<string, string[]>
                    {
                        {
                            "attendance",
                            [
                                "Check-in required first"
                            ]
                        }
                    }
                );
        }

        if (attendance.CheckOutTime != null)
        {
            throw new
                AppValidationException(
                    "Validation failed",
                    new Dictionary<string, string[]>
                    {
                        {
                            "attendance",
                            [
                                "Already checked out"
                            ]
                        }
                    });
        }

        var now = DateTime.UtcNow;
        attendance.CheckOutTime = now;
        var workedHours = now - attendance.CheckInTime;

        if (workedHours.TotalHours < 4)
        {
            attendance.Status = AttendanceStatus.HalfDay;
        }

        await context.SaveChangesAsync();
    }

    public async Task<PagedResult<AttendanceDto>> GetAllAsync(PaginationParams param)
    {
        var query =
            context.Attendances
                .AsNoTracking();

        if (currentUser.Role == UserRole.Employee.ToString())
        {
            var employeeId = currentUser.EmployeeId
                ?? throw new UnauthorizedAccessException("Unauthorized");

            query = query.Where(x => x.EmployeeId == employeeId);
        }

        var totalCount =
            await query
                .CountAsync();

        var attendances =
            await query
                .Include(x =>
                    x.Employee)
                .OrderByDescending(x =>
                    x.Date)
                .Skip(
                    (param.Page - 1)
                    * param
                        .PageSize)
                .Take(
                    param
                        .PageSize)
                .ProjectTo<
                    AttendanceDto>(
                    mapper
                        .ConfigurationProvider)
                .ToListAsync();

        return new
            PagedResult<
                AttendanceDto>(
                attendances,
                param.Page,
                param.PageSize,
                totalCount
            );
    }

    public async Task<IEnumerable<AttendanceDto>> GetEmployeeAttendanceAsync(int employeeId)
    {
        if (
            currentUser.Role == UserRole.Employee.ToString()
            && currentUser.EmployeeId != employeeId
        )
        {
            throw new UnauthorizedAccessException("Unauthorized");
        }

        return await context
            .Attendances
            .Where(x =>
                x.EmployeeId
                == employeeId)
            .Include(x =>
                x.Employee)
            .ProjectTo<
                AttendanceDto>(
                mapper
                    .ConfigurationProvider)
            .ToListAsync();
    }
}
