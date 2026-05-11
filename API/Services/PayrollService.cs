using API.Data;
using API.DTOs.Payroll;
using API.Exceptions;
using API.Models;
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
        var exists =
            await context.Payrolls
                .AnyAsync(x =>
                    x.EmployeeId
                    == dto.EmployeeId
                    && x.PayPeriodStart
                    == dto.PayPeriodStart
                    && x.PayPeriodEnd
                    == dto.PayPeriodEnd);

        if (exists)
        {
            throw new
                AppValidationException(
                    "Validation failed",
                    new Dictionary< string, string[]>
                    {
                        {
                            "payroll",
                            [
                                "Payroll already generated for this period"
                            ]
                        }
                    });
        }

        var payroll =
            new Payroll
            {
                EmployeeId = dto.EmployeeId,

                PayPeriodStart = dto.PayPeriodStart,

                PayPeriodEnd = dto.PayPeriodEnd,

                GrossSalary = dto.GrossSalary,

                TotalBonus = dto.TotalBonus,

                TotalDeductions = dto.TotalDeductions,

                TaxAmount = dto.TaxAmount,

                NetSalary =
                    dto.GrossSalary
                    + dto.TotalBonus
                    - dto.TotalDeductions
                    - dto.TaxAmount,

                GeneratedAt = DateTime.UtcNow,

                Status = PayrollStatus.Pending
            };

        context.Payrolls
            .Add(payroll);

        await context
            .SaveChangesAsync();
    }

    public async Task<PagedResult<PayrollDto>> GetAllAsync(PaginationParams param)
    {
        var query =
            context.Payrolls
                .AsNoTracking();

        var totalCount =
            await query
                .CountAsync();

        var payrolls =
            await query
                .Include(x =>
                    x.Employee)
                .OrderByDescending(x =>
                    x.GeneratedAt)
                .Skip(
                    (param.Page - 1)
                    * param.PageSize)
                .Take(
                    param.PageSize)
                .ProjectTo<
                    PayrollDto>(
                    mapper
                        .ConfigurationProvider)
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
