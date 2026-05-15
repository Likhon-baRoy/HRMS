using API.Models;
using API.Models.Enums;
using API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace API.Data.Seeders;

public static class DbSeeder
{
    public static async Task SeedAsync(
        AppDbContext context
    )
    {
        var alreadySeeded =
            await context.UserAccounts
                .IgnoreQueryFilters()
                .AnyAsync();

        if (alreadySeeded)
        {
            return;
        }

        var passwordService =
            context.GetService<
                IPasswordService>();

        // =====================
        // Departments
        // =====================

        var departments =
            new List<Department>
            {
                new()
                {
                    Name =
                        "Administration",
                    Description =
                        "System Administration"
                },

                new()
                {
                    Name =
                        "Human Resource",
                    Description =
                        "HR Department"
                },

                new()
                {
                    Name =
                        "Accounts",
                    Description =
                        "Finance Department"
                },

                new()
                {
                    Name =
                        "IT",
                    Description =
                        "Technology Department"
                },

                new()
                {
                    Name =
                        "Operations",
                    Description =
                        "Operations Team"
                }
            };

        context.Departments
            .AddRange(
                departments
            );

        await context
            .SaveChangesAsync();

        // =====================
        // Positions
        // =====================

        var positions =
            new List<Position>
            {
                new()
                {
                    Title =
                        "System Administrator",

                    JobLevel =
                        "Senior",

                    DepartmentId =
                        departments[0].Id
                },

                new()
                {
                    Title =
                        "HR Manager",

                    JobLevel =
                        "Manager",

                    DepartmentId =
                        departments[1].Id
                },

                new()
                {
                    Title =
                        "Accountant",

                    JobLevel =
                        "Mid",

                    DepartmentId =
                        departments[2].Id
                },

                new()
                {
                    Title =
                        "Software Engineer",

                    JobLevel =
                        "Mid",

                    DepartmentId =
                        departments[3].Id
                },

                new()
                {
                    Title =
                        "Operations Executive",

                    JobLevel =
                        "Junior",

                    DepartmentId =
                        departments[4].Id
                }
            };

        context.Positions
            .AddRange(
                positions
            );

        await context
            .SaveChangesAsync();

        // =====================
        // Employees
        // =====================

        var employees =
            new List<Employee>
            {
                new()
                {
                    EmployeeCode =
                        "EMP-0001",

                    FirstName =
                        "System",

                    LastName =
                        "Admin",

                    Email =
                        "admin@hrms.com",

                    Phone =
                        "01700000001",

                    Address =
                        "Head Office",

                    DateOfBirth =
                        new DateTime(
                            1990,
                            1,
                            1
                        ),

                    HireDate =
                        DateTime.UtcNow,

                    EmploymentType =
                        EmploymentType.Permanent,

                    EmployeeStatus =
                        EmployeeStatus.Active,

                    DepartmentId =
                        departments[0].Id,

                    PositionId =
                        positions[0].Id
                },

                new()
                {
                    EmployeeCode =
                        "EMP-0002",

                    FirstName =
                        "Sarah",

                    LastName =
                        "Khan",

                    Email =
                        "hr@hrms.com",

                    Phone =
                        "01700000002",

                    Address =
                        "Dhaka",

                    DateOfBirth =
                        new DateTime(
                            1992,
                            5,
                            15
                        ),

                    HireDate =
                        DateTime.UtcNow,

                    EmploymentType =
                        EmploymentType.Permanent,

                    EmployeeStatus =
                        EmployeeStatus.Active,

                    DepartmentId =
                        departments[1].Id,

                    PositionId =
                        positions[1].Id
                },

                new()
                {
                    EmployeeCode =
                        "EMP-0003",

                    FirstName =
                        "John",

                    LastName =
                        "Doe",

                    Email =
                        "manager@hrms.com",

                    Phone =
                        "01700000003",

                    Address =
                        "Khulna",

                    DateOfBirth =
                        new DateTime(
                            1994,
                            2,
                            10
                        ),

                    HireDate =
                        DateTime.UtcNow,

                    EmploymentType =
                        EmploymentType.Permanent,

                    EmployeeStatus =
                        EmployeeStatus.Active,

                    DepartmentId =
                        departments[3].Id,

                    PositionId =
                        positions[3].Id
                },

                new()
                {
                    EmployeeCode =
                        "EMP-0004",

                    FirstName =
                        "Employee",

                    LastName =
                        "User",

                    Email =
                        "employee@hrms.com",

                    Phone =
                        "01700000004",

                    Address =
                        "Chittagong",

                    DateOfBirth =
                        new DateTime(
                            1998,
                            8,
                            20
                        ),

                    HireDate =
                        DateTime.UtcNow,

                    EmploymentType =
                        EmploymentType.Probation,

                    EmployeeStatus =
                        EmployeeStatus.Active,

                    DepartmentId =
                        departments[4].Id,

                    PositionId =
                        positions[4].Id
                }
            };

        context.Employees
            .AddRange(
                employees
            );

        await context
            .SaveChangesAsync();

        // =====================
        // User Accounts
        // =====================

        var users =
            new List<UserAccount>
            {
                new()
                {
                    EmployeeId =
                        employees[0].Id,

                    Username =
                        "admin",

                    PasswordHash =
                        passwordService.Hash(
                            "Admin@123"
                        ),

                    Role =
                        UserRole.Admin
                },

                new()
                {
                    EmployeeId =
                        employees[1].Id,

                    Username =
                        "hr",

                    PasswordHash =
                        passwordService.Hash(
                            "Hr@123"
                        ),

                    Role =
                        UserRole.Hr
                },

                new()
                {
                    EmployeeId =
                        employees[2].Id,

                    Username =
                        "manager",

                    PasswordHash =
                        passwordService.Hash(
                            "Manager@123"
                        ),

                    Role =
                        UserRole.Manager
                },

                new()
                {
                    EmployeeId =
                        employees[3].Id,

                    Username =
                        "employee",

                    PasswordHash =
                        passwordService.Hash(
                            "Employee@123"
                        ),

                    Role =
                        UserRole.Employee
                }
            };

        context.UserAccounts
            .AddRange(
                users
            );

        await context
            .SaveChangesAsync();
    }
}
