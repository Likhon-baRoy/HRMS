using API.DTOs;
using API.Requests;
using API.Responses;

namespace API.Services.Interfaces;

public interface IEmployeeService
{
    Task<PagedResult<EmployeeDto>> GetAllAsync(PaginationParams param);

    Task<EmployeeDto> GetByIdAsync(int id);

    Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto);

    Task UpdateAsync(int id, UpdateEmployeeDto dto);

    Task DeleteAsync(int id);
}