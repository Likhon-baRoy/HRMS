using API.DTOs.Salary;
using API.Requests;
using API.Responses;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/salary")]
[Authorize(Roles = "Admin,Hr")]
public class SalaryController(ISalaryService service) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<SalaryDto>>> GetAll([FromQuery] PaginationParams param)
    {
        var result = await service.GetAllAsync(param);

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<object>>> Create(CreateSalaryDto dto)
    {
        await service.CreateAsync(dto);

        return Success("Salary assigned successfully");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> Update(int id, UpdateSalaryDto dto)
    {
        await service.UpdateAsync(id, dto);

        return Success("Salary updated successfully");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        await service.DeleteAsync(id);

        return Success("Salary removed successfully");
    }
}
