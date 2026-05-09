using API.DTOs.Departments;
using API.Requests;
using API.Responses;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Roles = "Admin")]
public class DepartmentController(IDepartmentService service) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<DepartmentDto>>> GetAll([FromQuery] PaginationParams param)
    {
        var result = await service.GetAllAsync(param);

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<DepartmentDto>>> GetById(int id)
    {
        var department = await service.GetByIdAsync(id);

        return Success(department);
    }

    [HttpPost]
    public async Task<ActionResult> Create(CreateDepartmentDto dto)
    {
        var department = await service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = department.Id },
            ApiResponse<DepartmentDto>.Ok(department)
        );
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, UpdateDepartmentDto dto)
    {
        await service.UpdateAsync(id, dto);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        await service.DeleteAsync((id));

        return NoContent();
    }
}
