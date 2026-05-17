using API.DTOs.Payroll;
using API.Requests;
using API.Responses;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Roles = "Admin,Hr")]
public class PayrollController(IPayrollService service) : BaseApiController
{
    [HttpPost]
    public async Task<ActionResult<ApiResponse<object>>> Generate([FromBody] GeneratePayrollDto dto)
    {
        await service
            .GenerateAsync(dto);

        return Success("Payroll generated successfully");
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<PayrollDto>>> GetAll([FromQuery] PaginationParams param)
    {
        var result =
            await service
                .GetAllAsync(param);

        return Ok(result);
    }

    [HttpGet("employee/{employeeId:int}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<PayrollDto>>>> GetEmployeePayroll(int employeeId)
    {
        var result =
            await service
                .GetEmployeePayrollAsync(employeeId);

        return Success(result);
    }
}
