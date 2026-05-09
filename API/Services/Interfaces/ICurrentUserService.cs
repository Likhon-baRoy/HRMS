namespace API.Services.Interfaces;

public interface ICurrentUserService
{
    int? UserId { get; }
    int? EmployeeId { get; }
    string? Username { get; }
    string? Role { get; }
    bool IsAuthenticated { get; }
}
