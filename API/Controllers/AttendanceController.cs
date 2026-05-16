using API.DTOs.Attendance;
using API.Requests;
using API.Responses;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class AttendanceController(IAttendanceService service) : BaseApiController
{
    [HttpPost("check-in")]
    public async Task<ActionResult<ApiResponse<object>>> CheckIn(CheckInDto dto)
    {
        await service.CheckInAsync(dto);

        return Success("Checked in successfully");
    }

    [HttpPost("check-out")]
    public async Task<ActionResult<ApiResponse<object>>> CheckOut()
    {
        await service.CheckOutAsync();

        return Success("Checked out successfully");
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<AttendanceDto>>> GetAll([FromQuery] PaginationParams param)
    {
        var result =
            await service
                .GetAllAsync(
                    param);

        return Ok(result);
    }

    [HttpGet("employee/{employeeId:int}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<AttendanceDto>>>> GetEmployeeAttendance(int employeeId)
    {
        var result =
            await service
                .GetEmployeeAttendanceAsync(
                    employeeId);

        return Success(result);
    }
}
