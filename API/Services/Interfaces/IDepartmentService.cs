using API.DTOs.Departments;
using API.Requests;
using API.Responses;

namespace API.Services.Interfaces;

public interface IDepartmentService
{
    Task<PagedResult<DepartmentDto>> GetAllAsync(PaginationParams param);

    Task<DepartmentDto> GetByIdAsync(int id);

    Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto);

    Task UpdateAsync(int id, UpdateDepartmentDto dto);

    Task DeleteAsync(int id);
}
