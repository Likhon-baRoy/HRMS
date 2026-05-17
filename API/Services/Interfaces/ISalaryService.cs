using API.DTOs.Salary;
using API.Requests;
using API.Responses;

namespace API.Services.Interfaces;

public interface ISalaryService
{
  Task<PagedResult<SalaryDto>> GetAllAsync(
      PaginationParams pagination
  );

  Task CreateAsync(
      CreateSalaryDto dto
  );

  Task UpdateAsync(
      int id,
      UpdateSalaryDto dto
  );

  Task DeleteAsync(
      int id
  );
}