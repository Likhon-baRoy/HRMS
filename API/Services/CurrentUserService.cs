using System.Security.Claims;
using API.Services.Interfaces;

namespace API.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public int? UserId
    {
        get
        {
            var value = httpContextAccessor
                .HttpContext?
                .User?
                .FindFirstValue(ClaimTypes.NameIdentifier);

            return int.TryParse(value, out var id) ? id : null;
        }
    }

    public int? EmployeeId
    {
        get
        {
            var value = httpContextAccessor
                .HttpContext?
                .User?
                .FindFirst("employeeId")?
                .Value;

            return int.TryParse(value, out var id) ? id : null;
        }
    }

    public string? Username =>
        httpContextAccessor.HttpContext?
        .User?
        .FindFirstValue(ClaimTypes.Name);

    public string? Role =>
        httpContextAccessor.HttpContext?
        .User?
        .FindFirstValue(ClaimTypes.Role);

    public bool IsAuthenticated =>
        httpContextAccessor.HttpContext?
        .User?
        .Identity?
        .IsAuthenticated ?? false;
}
