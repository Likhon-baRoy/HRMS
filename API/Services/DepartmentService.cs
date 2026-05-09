using API.Data;
using API.DTOs.Departments;
using API.Requests;
using API.Responses;
using API.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class DepartmentService(AppDbContext context, IMapper mapper) : IDepartmentService
{
    public async Task<PagedResult<DepartmentDto>> GetAllAsync(PaginationParams param)
    {
        var query = context.Departments.AsQueryable();

        var totalCount = await query.CountAsync();

        var departments = await query
            .OrderByDescending(d => d.Id)
            .Skip((param.Page - 1) * param.PageSize)
            .Take(param.PageSize)
            .ToListAsync();

        var data = mapper.Map<IEnumerable<DepartmentDto>>(departments);

        return new PagedResult<DepartmentDto>
        {
            Items = data,
            Meta = new PaginationMeta
            {
                Page = param.Page,
                PageSize = param.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((totalCount / (double)param.PageSize))
            }
        };
    }

    public Task<DepartmentDto> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(int id, UpdateDepartmentDto dto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}
