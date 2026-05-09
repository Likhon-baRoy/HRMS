using API.Data;
using API.DTOs.Departments;
using API.Exceptions;
using API.Models;
using API.Requests;
using API.Responses;
using API.Services.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class DepartmentService(AppDbContext context, IMapper mapper) : IDepartmentService
{
    public async Task<PagedResult<DepartmentDto>> GetAllAsync(PaginationParams param)
    {
        var query = context.Departments
            .AsNoTracking();

        var totalCount = await query.CountAsync();

        var departments = await query
            .OrderByDescending(d => d.Id)
            .Skip((param.Page - 1) * param.PageSize)
            .Take(param.PageSize)
            .ProjectTo<DepartmentDto>(mapper.ConfigurationProvider)
            .ToListAsync();

        return new PagedResult<DepartmentDto>(
            departments,
            param.Page,
            param.PageSize,
            totalCount
        );
    }

    public async Task<DepartmentDto> GetByIdAsync(int id)
    {
        var department = await context.Departments
                             .AsNoTracking()
                             .ProjectTo<DepartmentDto>(mapper.ConfigurationProvider)
                             .FirstOrDefaultAsync(d => d.Id == id)
                         ?? throw new NotFoundException("Department not found");

        return department;
    }

    public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto)
    {
        var department = mapper.Map<Department>(dto);

        context.Departments.Add(department);

        await context.SaveChangesAsync();

        return await GetByIdAsync(department.Id);
    }

    public async Task UpdateAsync(int id, UpdateDepartmentDto dto)
    {
        var department = await context.Departments.FindAsync(id) ?? throw new NotFoundException("Department not found");

        mapper.Map(dto, department);

        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var department = await context.Departments.FindAsync(id) ?? throw new NotFoundException("Department not found");

        var hasEmployees = await context.Employees
            .AnyAsync(x => x.DepartmentId == id);

        if (hasEmployees)
        {
            throw new AppValidationException(
                "Validation failed",
                new Dictionary<string, string[]>
                {
                    {
                        "department",
                        ["Department contains employees"]
                    }
                }
            );
        }

        department.IsDeleted = true;

        await context.SaveChangesAsync();
    }
}
