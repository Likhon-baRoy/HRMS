using API.DTOs.Dashboard;
using API.Responses;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class DashboardController(IDashboardService service) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<DashboardStatsDto>>> GetStats()
    {
        var result =
            await service
                .GetStatsAsync();

        return Success(result);
    }
}
