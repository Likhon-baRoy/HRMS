using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Data;
using API.DTOs;
using API.Exceptions;
using API.Extensions;
using API.Models;
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
    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await context.UserAccounts
            .Include(x => x.Employee)
            .FirstOrDefaultAsync(x => x.Username == dto.Username);

        if (user == null || !user.IsActive)
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        var validPassword = passwordService.verify(dto.Password!, user.PasswordHash);

        if (!validPassword)
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        user.LastLoginAt = DateTime.UtcNow;

        await context.SaveChangesAsync();

        var token = GenerateToken(user);

        return token;
    }

    public async Task RegisterAsync(RegisterUserDto dto)
    {
        await context.Employees
            .GetByIdOrThrowAsync(
                dto.EmployeeId);

        var alreadyLinked =
            await context.UserAccounts
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
            throw new UnauthorizedAccessException(
                "Unauthorized");
        }

        return new CurrentUserDto
        {
            UserId = currentUser.UserId ?? 0,

            EmployeeId = currentUser.EmployeeId ?? 0,

            Username = currentUser.Username ?? string.Empty,

            Role = currentUser.Role ?? string.Empty
        };
    }

    private AuthResponseDto GenerateToken(UserAccount user)
    {
        var expiryDays = config.GetValue<int>("Jwt:ExpiryInDays");

        var expiresAt = DateTime.UtcNow.AddDays(expiryDays);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),

            new(ClaimTypes.Name, user.Username),

            new(ClaimTypes.Role, user.Role),

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
            Token =
                new JwtSecurityTokenHandler()
                    .WriteToken(token),

            Username = user.Username,

            Role = user.Role,

            ExpiresAt = expiresAt
        };
    }
}
