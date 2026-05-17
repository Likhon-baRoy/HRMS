using API.Data;
using API.DTOs.Salary;
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

public class SalaryService(
    AppDbContext context,
    IMapper mapper,
    ICurrentUserService currentUser
) : ISalaryService
{
    public async Task<PagedResult<SalaryDto>> GetAllAsync(PaginationParams pagination)
    {
        var query = context.Salaries
            .AsNoTracking()
            .Include(x => x.Employee)
            .Where(x => x.IsCurrent);

        if (pagination.EmployeeId.HasValue)
        {
            query = query.Where(x => x.EmployeeId == pagination.EmployeeId.Value);
        }

        var totalCount = await query.CountAsync();

        var salaries = await query
            .OrderByDescending(x => x.EffectiveDate)
            .ThenByDescending(x => x.Id)
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ProjectTo<SalaryDto>(mapper.ConfigurationProvider)
            .ToListAsync();

        return new PagedResult<SalaryDto>(
            salaries,
            pagination.Page,
            pagination.PageSize,
            totalCount
        );
    }

    public async Task CreateAsync(CreateSalaryDto dto)
    {
        await ApplySalaryChangeAsync(
            dto.EmployeeId,
            dto.BasicSalary,
            dto.HouseRent,
            dto.MedicalAllowance,
            dto.TransportAllowance,
            dto.OtherAllowance,
            dto.EffectiveDate,
            reason: "Initial salary assignment"
        );
    }

    public async Task UpdateAsync(int id, UpdateSalaryDto dto)
    {
        var existing = await context.Salaries
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new NotFoundException(nameof(Salary), id);

        if (!existing.IsCurrent)
        {
            throw new AppValidationException(
                "Validation failed",
                new Dictionary<string, string[]>
                {
                    { "salary", ["Only current salary can be revised"] }
                }
            );
        }

        if (dto.EmployeeId != existing.EmployeeId)
        {
            throw new AppValidationException(
                "Validation failed",
                new Dictionary<string, string[]>
                {
                    { "employeeId", ["Salary employee cannot be changed during revision"] }
                }
            );
        }

        await ApplySalaryChangeAsync(
            dto.EmployeeId,
            dto.BasicSalary,
            dto.HouseRent,
            dto.MedicalAllowance,
            dto.TransportAllowance,
            dto.OtherAllowance,
            dto.EffectiveDate,
            reason: "Salary revision",
            salaryToReplace: existing
        );
    }

    public async Task DeleteAsync(int id)
    {
        var salary = await context.Salaries
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new NotFoundException(nameof(Salary), id);

        if (!salary.IsCurrent)
        {
            throw new AppValidationException(
                "Validation failed",
                new Dictionary<string, string[]>
                {
                    { "salary", ["Only current salary can be removed"] }
                }
            );
        }

        salary.IsCurrent = false;
        salary.EndDate = DateTime.UtcNow;

        await context.SaveChangesAsync();
    }

    private async Task ApplySalaryChangeAsync(
        int employeeId,
        decimal basicSalary,
        decimal houseRent,
        decimal medicalAllowance,
        decimal transportAllowance,
        decimal otherAllowance,
        DateTime effectiveDate,
        string reason,
        Salary? salaryToReplace = null
    )
    {
        var effectiveDateUtc = NormalizeToUtc(effectiveDate);

        var employeeExists = await context.Employees
            .AnyAsync(x => x.Id == employeeId);

        if (!employeeExists)
        {
            throw new AppValidationException(
                "Validation failed",
                new Dictionary<string, string[]>
                {
                    { "employeeId", ["Employee not found"] }
                }
            );
        }

        var currentSalary = salaryToReplace
            ?? await context.Salaries
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.EmployeeId == employeeId && x.IsCurrent);

        if (currentSalary != null)
        {
            currentSalary.IsCurrent = false;
            currentSalary.EndDate = effectiveDateUtc.AddDays(-1);

            context.SalaryRevisions.Add(new SalaryRevision
            {
                SalaryId = currentSalary.Id,
                OldGrossSalary = currentSalary.GrossSalary,
                NewGrossSalary = CalculateGrossSalary(
                    basicSalary,
                    houseRent,
                    medicalAllowance,
                    transportAllowance,
                    otherAllowance
                ),
                Reason = reason,
                RevisionDate = DateTime.UtcNow,
                ApprovedBy = currentUser.UserId ?? 0
            });
        }

        var salary = mapper.Map<Salary>(new CreateSalaryDto
        {
            EmployeeId = employeeId,
            BasicSalary = basicSalary,
            HouseRent = houseRent,
            MedicalAllowance = medicalAllowance,
            TransportAllowance = transportAllowance,
            OtherAllowance = otherAllowance,
            EffectiveDate = effectiveDateUtc
        });

        context.Salaries.Add(salary);

        await context.SaveChangesAsync();
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

    private static decimal CalculateGrossSalary(
        decimal basicSalary,
        decimal houseRent,
        decimal medicalAllowance,
        decimal transportAllowance,
        decimal otherAllowance
    )
    {
        return basicSalary
            + houseRent
            + medicalAllowance
            + transportAllowance
            + otherAllowance;
    }
}
