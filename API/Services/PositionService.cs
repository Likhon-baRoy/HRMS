using API.Data;
using API.DTOs.Positions;
using API.Exceptions;
using API.Extensions;
using API.Models;
using API.Requests;
using API.Responses;
using API.Services.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class PositionService(AppDbContext context, IMapper mapper) : IPositionService
{
    public async Task<PagedResult<PositionDto>> GetAllAsync( PaginationParams param)
    {
        var query =
            context.Positions
                .AsNoTracking();

        var totalCount =
            await query.CountAsync();

        var positions =
            await query
                .Include(x =>
                    x.Department)
                .OrderByDescending(x => x.Id)
                .Skip(
                    (param.Page - 1)
                    * param.PageSize)
                .Take(
                    param.PageSize)
                .ProjectTo<
                    PositionDto>(
                    mapper.ConfigurationProvider)
                .ToListAsync();

        return new
            PagedResult<PositionDto>(
                positions,
                param.Page,
                param.PageSize,
                totalCount
            );
    }

    public async Task<PositionDto> GetByIdAsync(int id)
    {
        return await context
                   .Positions
                   .AsNoTracking()
                   .Where(x => x.Id == id)
                   .ProjectTo<
                       PositionDto>(
                       mapper.ConfigurationProvider)
                   .FirstOrDefaultAsync()
               ?? throw new
                   NotFoundException(
                       nameof(Position),
                       id);
    }

    public async Task<PositionDto> CreateAsync( CreatePositionDto dto)
    {
        var position =
            mapper.Map<Position>(
                dto);

        context.Positions
            .Add(position);

        await context
            .SaveChangesAsync();

        return await
            GetByIdAsync(
                position.Id);
    }

    public async Task UpdateAsync( int id, UpdatePositionDto dto)
    {
        var position =
            await context
                .Positions
                .GetByIdOrThrowAsync(id);

        if (!string.IsNullOrWhiteSpace(dto.Title))
        {
            position.Title =
                dto.Title;
        }

        if (!string.IsNullOrWhiteSpace(dto.JobLevel))
        {
            position.JobLevel =
                dto.JobLevel;
        }

        if (dto.DepartmentId.HasValue)
        {
            position.DepartmentId =
                dto.DepartmentId.Value;
        }

        await context
            .SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var position =
            await context
                .Positions
                .GetByIdOrThrowAsync(
                    id);

        var hasEmployees =
            await context
                .Employees
                .AnyAsync(x =>
                    x.PositionId == id);

        if (hasEmployees)
        {
            throw new
                AppValidationException(
                    "Validation failed",
                    new Dictionary<
                        string,
                        string[]>
                    {
                        {
                            "position",
                            [
                                "Position contains employees"
                            ]
                        }
                    });
        }

        position.IsDeleted =
            true;

        await context
            .SaveChangesAsync();
    }
}
