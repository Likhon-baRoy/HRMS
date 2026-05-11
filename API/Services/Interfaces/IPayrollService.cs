using API.DTOs.Payroll;
using API.Requests;
using API.Responses;

namespace API.Services.Interfaces;

public interface IPayrollService
{
    Task GenerateAsync(GeneratePayrollDto dto);

    Task<PagedResult<PayrollDto>> GetAllAsync(PaginationParams param);

    Task<IEnumerable<PayrollDto>> GetEmployeePayrollAsync(int employeeId);
}
