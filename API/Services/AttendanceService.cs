using API.Data;
using API.DTOs.Attendance;
using API.Exceptions;
using API.Models;
using API.Requests;
using API.Responses;
using API.Services.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class AttendanceService(AppDbContext context, IMapper mapper) : IAttendanceService
{
    public async Task CheckInAsync(CheckInDto dto)
    {
        var today =
            DateOnly.FromDateTime(
                DateTime.UtcNow);

        var exists =
            await context
                .Attendances
                .AnyAsync(x =>
                    x.EmployeeId
                    == dto.EmployeeId
                    && x.Date
                    == today);

        if (exists)
        {
            throw new
                AppValidationException(
                    "Validation failed",
                    new Dictionary<
                        string,
                        string[]>
                    {
                        {
                            "attendance",
                            [
                                "Already checked in today"
                            ]
                        }
                    });
        }

        var attendance =
            new Attendance
            {
                EmployeeId =
                    dto.EmployeeId,

                Date = today,

                CheckInTime =
                    DateTime.UtcNow,

                Remarks =
                    dto.Remarks
            };

        context.Attendances
            .Add(attendance);

        await context
            .SaveChangesAsync();
    }

    public async Task CheckOutAsync(CheckOutDto dto)
    {
        var today =
            DateOnly.FromDateTime(
                DateTime.UtcNow);

        var attendance =
            await context
                .Attendances
                .FirstOrDefaultAsync(x =>
                    x.EmployeeId
                    == dto.EmployeeId
                    && x.Date
                    == today)
            ?? throw new NotFoundException(
                nameof(Attendance),
                dto.EmployeeId);

        if (attendance
                .CheckOutTime
            != null)
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

        attendance.CheckOutTime = DateTime.UtcNow;

        await context.SaveChangesAsync();
    }

    public async Task<PagedResult<AttendanceDto>> GetAllAsync(PaginationParams param)
    {
        var query =
            context.Attendances
                .AsNoTracking();

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
