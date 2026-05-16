using API.DTOs.Attendance;
using API.Requests;
using API.Responses;

namespace API.Services.Interfaces;

public interface IAttendanceService
{
    Task CheckInAsync(CheckInDto dto);

    Task CheckOutAsync();

    Task<PagedResult<AttendanceDto>> GetAllAsync(PaginationParams param);

    Task<IEnumerable<AttendanceDto>> GetEmployeeAttendanceAsync(int employeeId);
}
