using API.Data;
using API.DTOs.Payroll;
using API.Exceptions;
using API.Models;
using API.Models.Enums;
using API.Requests;
using API.Responses;
using API.Services.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class PayrollService(AppDbContext context, IMapper mapper) : IPayrollService
{
    public async Task GenerateAsync(GeneratePayrollDto dto)
    {
        var payPeriodStart = NormalizeToUtc(dto.PayPeriodStart);
        var payPeriodEnd = NormalizeToUtc(dto.PayPeriodEnd);

        var activeEmployees = await context.Employees
            .AsNoTracking()
            .Where(x => x.EmployeeStatus == EmployeeStatus.Active)
            .Select(x => x.Id)
            .ToListAsync();

        var generatedCount = 0;
        var skippedNoSalary = 0;
        var skippedExisting = 0;

        foreach (var employeeId in activeEmployees)
        {
            var alreadyGenerated = await context.Payrolls.AnyAsync(x =>
                x.EmployeeId == employeeId &&
                x.PayPeriodStart == payPeriodStart &&
                x.PayPeriodEnd == payPeriodEnd);

            if (alreadyGenerated)
            {
                skippedExisting++;
                continue;
            }

            var salary = await context.Salaries
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.EmployeeId == employeeId &&
                    x.IsCurrent);

            if (salary == null)
            {
                skippedNoSalary++;
                continue;
            }

            var payroll = new Payroll
            {
                EmployeeId = employeeId,
                PayPeriodStart = payPeriodStart,
                PayPeriodEnd = payPeriodEnd,
                GrossSalary = salary.GrossSalary,
                TotalBonus = dto.BonusAmount,
                TotalDeductions = dto.DeductionAmount,
                TaxAmount = Math.Round(salary.GrossSalary * dto.TaxPercent / 100m, 2),
                GeneratedAt = DateTime.UtcNow,
                Status = PayrollStatus.Processed
            };

            payroll.CalculateNetSalary();

            context.Payrolls.Add(payroll);
            generatedCount++;
        }

        await context.SaveChangesAsync();

        if (generatedCount == 0)
        {
            throw new AppValidationException(
                "Validation failed",
                new Dictionary<string, string[]>
                {
                    {
                        "payroll",
                        [
                            "No payrolls were generated"
                        ]
                    }
                }
            );
        }
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

    public async Task<PagedResult<PayrollDto>> GetAllAsync(PaginationParams param)
    {
        var query = context.Payrolls.AsNoTracking();

        var totalCount = await query.CountAsync();

        var payrolls =
            await query
                .Include(x => x.Employee)
                .OrderByDescending(x => x.GeneratedAt)
                .Skip((param.Page - 1) * param.PageSize)
                .Take(param.PageSize)
                .ProjectTo<PayrollDto>(mapper.ConfigurationProvider)
                .ToListAsync();

        return new PagedResult<PayrollDto>(
            payrolls,
            param.Page,
            param.PageSize,
            totalCount
        );
    }

    public async Task<IEnumerable<PayrollDto>> GetEmployeePayrollAsync(int employeeId)
    {
        return await context.Payrolls
            .Where(x => x.EmployeeId == employeeId)
            .Include(x => x.Employee)
            .ProjectTo<PayrollDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }
}
