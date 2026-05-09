using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Seeders;

public class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        await context.Database.MigrateAsync();

        if (await context.UserAccounts.AnyAsync()) return;

        // =========================
        // Department
        // =========================

        var department = new Department
        {
            Name = "Administration",
            Description = "System Administration"
        };

        context.Departments.Add(department);

        await context.SaveChangesAsync();

        // =========================
        // Position
        // =========================

        var position = new Position
        {
            Title = "System Administrator",
            JobLevel = "Senior",
            DepartmentId = department.Id
        };

        context.Positions.Add(position);

        await context.SaveChangesAsync();

        // =========================
        // Employee
        // =========================

        var employee = new Employee
        {
            EmployeeCode = "EMP-0001",

            FirstName = "System",

            LastName = "Administrator",

            Email = "admin@hrms.com",

            Phone = "000000000",

            Address = "System Generated",

            DateOfBirth = new DateTime(1990, 1, 1),

            HireDate = DateTime.UtcNow,

            EmploymentStatus = EmploymentStatus.Active,

            AccountNumber = "N/A",

            DepartmentId = department.Id,

            PositionId = position.Id
        };

        context.Employees.Add(employee);

        await context.SaveChangesAsync();

        // =========================
        // User Account
        // =========================

        var user = new UserAccount
        {
            EmployeeId = employee.Id,

            Username = "admin",

            PasswordHash =
                BCrypt.Net.BCrypt.HashPassword(
                    "Admin@123"),

            Role = "Admin",

            IsActive = true
        };

        context.UserAccounts.Add(user);

        await context.SaveChangesAsync();
    }
}
