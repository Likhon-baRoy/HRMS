using API.Data;
using API.DTOs;
using API.Exceptions;
using API.Extensions;
using API.Models;
using API.Models.Enums;
using API.Requests;
using API.Responses;
using API.Services.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class EmployeeService(AppDbContext context, IMapper mapper, ICurrentUserService currentUser) : IEmployeeService
{
    private readonly ICurrentUserService _currentUser = currentUser;

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
        return await context.Employees
                   .AsNoTracking()
                   .Where(x => x.Id == id)
                   .ProjectTo<EmployeeDto>(mapper.ConfigurationProvider)
                   .FirstOrDefaultAsync()
               ?? throw new NotFoundException(
                   nameof(Employee),
                   id);
    }

    public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto)
    {
        var employee = mapper.Map<Employee>(dto);

        context.Employees.Add(employee);

        await context.SaveChangesAsync();

        return await GetByIdAsync(employee.Id);
    }

    public async Task UpdateAsync(int id, UpdateEmployeeDto dto)
    {
        var employee =
            await context.Employees
                .GetByIdOrThrowAsync(id);

        if (!string.IsNullOrWhiteSpace(dto.Phone))
        {
            var phoneExists =
                await context.Employees
                    .AnyAsync(x => x.Id != id && x.Phone == dto.Phone && x.RecordStatus == RecordStatus.Active);

            if (phoneExists)
            {
                throw new AppValidationException(
                    "Validation failed",
                    new Dictionary<string, string[]>
                    {
                        {
                            "phone",
                            ["Phone already exists"]
                        }
                    }
                );
            }
        }

        mapper.Map(dto, employee);

        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        if (_currentUser.IsSelf(id))
        {
            throw new AppValidationException(
                "Validation failed",
                new Dictionary<string, string[]>
                {
                    {
                        "employee",
                        [
                            "You cannot delete your own account"
                        ]
                    }
                }
            );
        }

        var employee = await context.Employees
            .GetByIdOrThrowAsync(id);

        employee.RecordStatus = RecordStatus.Inactive;

        await context.SaveChangesAsync();
    }
}
