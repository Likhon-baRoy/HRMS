using API.DTOs.Positions;
using API.Requests;
using API.Responses;

namespace API.Services.Interfaces;

public interface IPositionService
{
    Task<PagedResult<PositionDto>>
        GetAllAsync(
            PaginationParams param);

    Task<PositionDto>
        GetByIdAsync(int id);

    Task<PositionDto>
        CreateAsync(
            CreatePositionDto dto);

    Task UpdateAsync(
        int id,
        UpdatePositionDto dto);

    Task DeleteAsync(int id);
}
