using API.DTOs.Positions;
using API.Requests;
using API.Responses;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Roles = "Admin")]
public class PositionController(IPositionService service) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<PositionDto>>> GetAll([FromQuery] PaginationParams param)
    {
        var result =
            await service
                .GetAllAsync(param);

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<PositionDto>>> GetById(int id)
    {
        var position =
            await service
                .GetByIdAsync(id);

        return Success(
            position);
    }

    [HttpPost]
    public async Task<ActionResult> Create(CreatePositionDto dto)
    {
        var position =
            await service
                .CreateAsync(dto);

        return
            CreatedAtAction(
                nameof(GetById),
                new
                {
                    id = position.Id
                },
                ApiResponse<PositionDto>.Ok(position)
            );
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, UpdatePositionDto dto)
    {
        await service
            .UpdateAsync(id, dto);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        await service
            .DeleteAsync(id);

        return NoContent();
    }
}
