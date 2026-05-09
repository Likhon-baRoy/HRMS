using API.Data;
using API.DTOs;
using API.Exceptions;
using API.Models;
using API.Requests;
using API.Responses;
using API.Services.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class EmployeeService(AppDbContext context, IMapper mapper) : IEmployeeService
{
    public async Task<PagedResult<EmployeeDto>> GetAllAsync(PaginationParams param)
    {
        var query = context.Employees
            .AsNoTracking();

        var totalCount = await query.CountAsync();

        var employees = await query
            .OrderByDescending(e => e.Id)
            .Skip((param.Page - 1) * param.PageSize)
            .Take(param.PageSize)
            .ProjectTo<EmployeeDto>(mapper.ConfigurationProvider)
            .ToListAsync();

        return new PagedResult<EmployeeDto>(
            employees,
            param.Page,
            param.PageSize,
            totalCount
        );
    }

    public async Task<EmployeeDto> GetByIdAsync(int id)
    {
        var employee = await context.Employees
            .AsNoTracking()
            .ProjectTo<EmployeeDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(e => e.Id == id)
            ?? throw new NotFoundException("Employee not found");

        return employee;
    }

    public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto)
    {
        var employee = mapper.Map<Employee>(dto);

        context.Employees.Add(employee);

        await context.SaveChangesAsync();

        return mapper.Map<EmployeeDto>(employee);
    }

    public async Task UpdateAsync(int id, UpdateEmployeeDto dto)
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
