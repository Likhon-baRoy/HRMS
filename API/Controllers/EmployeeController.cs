using API.DTOs;
using API.Requests;
using API.Responses;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Roles = "Admin")]
public class EmployeeController(IEmployeeService service) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<EmployeeDto>>> GetAll([FromQuery] PaginationParams param)
    {
        var result = await service.GetAllAsync(param);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<EmployeeDto>>> GetById(int id)
    {
        var employee = await service.GetByIdAsync(id);
        return Success(employee);
    }

    [HttpPost]
    public async Task<ActionResult> Create(CreateEmployeeDto dto)
    {
        var employee = await service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = employee.Id },
            ApiResponse<EmployeeDto>.Ok(employee)
        );
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, UpdateEmployeeDto dto)
    {
        await service.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }
}
