using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Data;
using API.DTOs;
using API.Exceptions;
using API.Extensions;
using API.Models;
using API.Models.Enums;
using API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class AuthService(
    AppDbContext context,
    IConfiguration config,
    IPasswordService passwordService,
    ICurrentUserService currentUser) : IAuthService
{
    private const string ProtectedAdminUsername = "admin";

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await context.UserAccounts
            .Include(x => x.Employee)
            .FirstOrDefaultAsync(x => x.Username == dto.Username);

        if (user == null)
            throw new UnauthorizedAccessException("Invalid credentials");

        if (
            user.Employee.EmployeeStatus
            is EmployeeStatus.Terminated
            or EmployeeStatus.Suspended
            or EmployeeStatus.Resigned
        )
        {
            throw new AccountDisabledException("Employee account is inactive");
        }

        var validPassword = passwordService.Verify(dto.Password!, user.PasswordHash);

        if (!validPassword)
            throw new UnauthorizedAccessException("Invalid credentials");

        user.LastLoginAt = DateTime.UtcNow;

        await context.SaveChangesAsync();

        var token = GenerateToken(user);

        return token;
    }

    public async Task RegisterAsync(RegisterUserDto dto)
    {
        var employee = await context.Employees
            .GetByIdOrThrowAsync(dto.EmployeeId);

        if (
            employee.EmployeeStatus
            is EmployeeStatus.Resigned
            or EmployeeStatus.Terminated
            or EmployeeStatus.Suspended
        )
        {
            throw new AppValidationException(
                "Validation failed",
                new Dictionary<string, string[]>
                {
                    {
                        "employeeId",
                        [
                            "Employee cannot have an account"
                        ]
                    }
                }
            );
        }

        var alreadyLinked = await context.UserAccounts
            .AnyAsync(x
                => x.EmployeeId == dto.EmployeeId);

        if (alreadyLinked)
        {
            throw new AppValidationException(
                "Validation failed",
                new Dictionary<string, string[]>
                {
                    {
                        "employeeId",
                        ["Employee already has an account"]
                    }
                });
        }

        var user = new UserAccount
        {
            EmployeeId = dto.EmployeeId,
            Username = dto.Username!,
            PasswordHash = passwordService.Hash(dto.Password!),
            Role = dto.Role
        };

        context.UserAccounts.Add(user);

        await context.SaveChangesAsync();
    }

    public CurrentUserDto GetCurrentUser()
    {
        if (!currentUser.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("Unauthorized");
        }

        return new CurrentUserDto
        {
            UserId = currentUser.UserId ?? 0,

            EmployeeId = currentUser.EmployeeId ?? 0,

            Username = currentUser.Username ?? string.Empty,

            RoleId = (int)Enum.Parse<UserRole>(currentUser.Role!),

            Role = Enum.Parse<UserRole>(currentUser.Role!).GetDisplayName(),
        };
    }

    public async Task<UserProfileDto> GetProfileAsync()
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
        {
            throw new UnauthorizedAccessException("Unauthorized");
        }

        var user = await context.UserAccounts
            .AsNoTracking()
            .Include(x => x.Employee)
                .ThenInclude(x => x.Department)
            .Include(x => x.Employee)
                .ThenInclude(x => x.Position)
            .FirstOrDefaultAsync(x => x.Id == currentUser.UserId.Value)
            ?? throw new UnauthorizedAccessException("Unauthorized");

        var salary = await context.Salaries
            .AsNoTracking()
            .Where(x => x.EmployeeId == user.EmployeeId && x.IsCurrent)
            .OrderByDescending(x => x.EffectiveDate)
            .FirstOrDefaultAsync();

        return new UserProfileDto
        {
            UserId = user.Id,
            EmployeeId = user.EmployeeId,
            Username = user.Username,
            RoleId = (int)user.Role,
            Role = user.Role.GetDisplayName(),
            EmployeeCode = user.Employee.EmployeeCode,
            FullName = $"{user.Employee.FirstName} {user.Employee.LastName}",
            Email = user.Employee.Email,
            Phone = user.Employee.Phone,
            Address = user.Employee.Address,
            HireDate = user.Employee.HireDate,
            EmploymentType = user.Employee.EmploymentType.GetDisplayName(),
            EmployeeStatus = user.Employee.EmployeeStatus.GetDisplayName(),
            AccountNumber = user.Employee.AccountNumber,
            DepartmentName = user.Employee.Department.Name,
            PositionTitle = user.Employee.Position.Title,
            BasicSalary = salary?.BasicSalary,
            GrossSalary = salary?.GrossSalary,
            SalaryEffectiveDate = salary?.EffectiveDate
        };
    }

    public async Task<List<UserAccountDto>> GetUsersAsync()
    {
        var users = await context.UserAccounts
            .AsNoTracking()
            .Include(x => x.Employee)
            .OrderBy(x => x.Employee.EmployeeCode)
            .ToListAsync();

        return users.Select(x => new UserAccountDto
        {
            Id = x.Id,
            EmployeeId = x.EmployeeId,
            EmployeeName = $"{x.Employee.FirstName} {x.Employee.LastName}",
            Username = x.Username,
            RoleId = (int)x.Role,
            Role = x.Role.GetDisplayName(),
            IsProtected = IsProtectedAccount(x)
        }).ToList();
    }

    public async Task UpdateRoleAsync(int id, UpdateUserRoleDto dto)
    {
        var user = await context.UserAccounts
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new NotFoundException(nameof(UserAccount), id);

        if (IsProtectedAccount(user))
        {
            throw new AppValidationException(
                "Validation failed",
                new Dictionary<string, string[]>
                {
                    { "role", ["Protected admin role cannot be changed"] }
                }
            );
        }

        if (currentUser.UserId == user.Id)
        {
            throw new AppValidationException(
                "Validation failed",
                new Dictionary<string, string[]>
                {
                    { "role", ["You cannot change your own role"] }
                }
            );
        }

        user.Role = dto.Role;

        await context.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await context.UserAccounts
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new NotFoundException(nameof(UserAccount), id);

        if (IsProtectedAccount(user))
        {
            throw new AppValidationException(
                "Validation failed",
                new Dictionary<string, string[]>
                {
                    { "user", ["Protected admin account cannot be deleted"] }
                }
            );
        }

        if (currentUser.UserId == user.Id)
        {
            throw new AppValidationException(
                "Validation failed",
                new Dictionary<string, string[]>
                {
                    { "user", ["You cannot delete your own account"] }
                }
            );
        }

        user.RecordStatus = RecordStatus.Deleted;

        await context.SaveChangesAsync();
    }

    private AuthResponseDto GenerateToken(UserAccount user)
    {
        var expiryDays = config.GetValue<int>("Jwt:ExpiryInDays");

        var expiresAt = DateTime.UtcNow.AddDays(expiryDays);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),

            new(ClaimTypes.Name, user.Username),

            new(ClaimTypes.Role, user.Role.ToString()),

            new("employeeId", user.EmployeeId.ToString())
        };

        var key =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    config["Jwt:Key"]!
                ));

        var credentials =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

        var token =
            new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

        return new AuthResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),

            Username = user.Username,

            RoleId = (int)user.Role,

            Role = user.Role.GetDisplayName(),

            ExpiresAt = expiresAt
        };
    }

    private static bool IsProtectedAccount(UserAccount user)
    {
        return user.Username.Equals(
            ProtectedAdminUsername,
            StringComparison.OrdinalIgnoreCase);
    }
}
