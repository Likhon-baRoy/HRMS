using API.Data;
using API.DTOs;
using API.Exceptions;
using API.Models;
using API.Requests;
using API.Responses;
using API.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class EmployeeService(AppDbContext context, IMapper mapper) : IEmployeeService
{
    public async Task<PagedResult<EmployeeDto>> GetAllAsync(PaginationParams param)
    {
        var query = context.Employees.AsQueryable();

        var totalCount = await query.CountAsync();

        var employees = await query
            .OrderByDescending(e => e.Id)
            .Skip((param.Page - 1) * param.PageSize)
            .Take(param.PageSize)
            .ToListAsync();

        var data = mapper.Map<IEnumerable<EmployeeDto>>(employees);

        return new PagedResult<EmployeeDto>
        {
            Items = data,
            Meta = new PaginationMeta
            {
                Page = param.Page,
                PageSize = param.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)param.PageSize)
            }
        };
    }

    public async Task<EmployeeDto> GetByIdAsync(int id)
    {
        var employee = await context.Employees.FindAsync(id) ?? throw new NotFoundException("Employee not found");
        return mapper.Map<EmployeeDto>(employee);
    }

    public async Task<EmployeeDto> CreateAsync(
        CreateEmployeeDto dto)
    {
        var employee = mapper.Map<Employee>(dto);

        context.Employees.Add(employee);

        await context.SaveChangesAsync();

        return mapper.Map<EmployeeDto>(employee);
    }

    public async Task UpdateAsync(
        int id,
        UpdateEmployeeDto dto)
    {
        var employee = await context.Employees.FindAsync(id) ?? throw new NotFoundException("Employee not found");
        mapper.Map(dto, employee);

        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var employee = await context.Employees.FindAsync(id) ?? throw new NotFoundException("Employee not found");
        context.Employees.Remove(employee);

        await context.SaveChangesAsync();
    }
}