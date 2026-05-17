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
    private const string ProtectedAdminUsername = "admin";

    private readonly ICurrentUserService _currentUser = currentUser;

    public async Task<PagedResult<EmployeeDto>> GetAllAsync(PaginationParams param)
    {
        var query = context.Employees
            .IgnoreQueryFilters()
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(param.Search))
        {
            var search = param.Search.Trim().ToLower();

            query = query.Where(x =>
                x.EmployeeCode.ToLower().Contains(search) ||
                x.FirstName.ToLower().Contains(search) ||
                x.LastName.ToLower().Contains(search) ||
                x.Email.ToLower().Contains(search) ||
                x.Phone.ToLower().Contains(search));
        }

        if (param.DepartmentId.HasValue)
        {
            query = query.Where(x => x.DepartmentId == param.DepartmentId.Value);
        }

        if (param.EmployeeStatus.HasValue)
        {
            query = query.Where(x => (int)x.EmployeeStatus == param.EmployeeStatus.Value);
        }
        else
        {
            query = query.Where(x =>
                x.EmployeeStatus != EmployeeStatus.Resigned &&
                x.EmployeeStatus != EmployeeStatus.Terminated);
        }

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

        NormalizeDates(employee);

        context.Employees.Add(employee);

        await context.SaveChangesAsync();

        return await GetByIdAsync(employee.Id);
    }

    public async Task UpdateAsync(int id, UpdateEmployeeDto dto)
    {
        var employee =
            await context.Employees
                .Include(x => x.UserAccount)
                .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new NotFoundException(nameof(Employee), id);

        if (!string.IsNullOrWhiteSpace(dto.Phone))
        {
            var phoneExists =
                await context.Employees
                    .AnyAsync(x =>
                        x.Id != dto.Id &&
                        x.Phone == dto.Phone &&
                        x.EmployeeStatus != EmployeeStatus.Resigned &&
                        x.EmployeeStatus != EmployeeStatus.Terminated);

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

        if (
            _currentUser.IsSelf(id) &&
            dto.EmployeeStatus.HasValue &&
            dto.EmployeeStatus.Value != employee.EmployeeStatus
        )
        {
            throw new AppValidationException(
                "Validation failed",
                new Dictionary<string, string[]>
                {
                    {
                        "employeeStatus",
                        [
                            "You cannot change your own status"
                        ]
                    }
                }
            );
        }

        if (
            IsProtectedAdminEmployee(employee) &&
            dto.EmployeeStatus.HasValue &&
            dto.EmployeeStatus.Value != employee.EmployeeStatus
        )
        {
            throw new AppValidationException(
                "Validation failed",
                new Dictionary<string, string[]>
                {
                    {
                        "employeeStatus",
                        [
                            "Protected admin status cannot be changed"
                        ]
                    }
                }
            );
        }

        mapper.Map(dto, employee);

        NormalizeDates(employee);

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
            .Include(x => x.UserAccount)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new NotFoundException(nameof(Employee), id);

        if (IsProtectedAdminEmployee(employee))
        {
            throw new AppValidationException(
                "Validation failed",
                new Dictionary<string, string[]>
                {
                    {
                        "employee",
                        [
                            "Protected admin employee cannot be deleted"
                        ]
                    }
                }
            );
        }

        employee.EmployeeStatus = EmployeeStatus.Terminated;

        await context.SaveChangesAsync();
    }

    private static void NormalizeDates(Employee employee)
    {
        employee.DateOfBirth = NormalizeToUtc(employee.DateOfBirth);
        employee.HireDate = NormalizeToUtc(employee.HireDate);
    }

    private static DateTime NormalizeToUtc(DateTime dateTime)
    {
        return dateTime.Kind switch
        {
            DateTimeKind.Utc => dateTime,
            DateTimeKind.Local => dateTime.ToUniversalTime(),
            _ => DateTime.SpecifyKind(dateTime, DateTimeKind.Utc)
        };
    }

    private static bool IsProtectedAdminEmployee(Employee employee)
    {
        return employee.UserAccount?.Username.Equals(
            ProtectedAdminUsername,
            StringComparison.OrdinalIgnoreCase) == true;
    }
}
