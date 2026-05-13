using API.Models;
using API.Models.Enums;
using API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace API.Data.Seeders;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        // already seeded
        var adminExists =
            await context.UserAccounts
                .IgnoreQueryFilters()
                .AnyAsync(x => x.Username == "admin");

        if (adminExists) return;

        // resolve password service
        var passwordService =
            context.GetService<
                IPasswordService>();

        // -------------------------
        // Department
        // -------------------------

        var department =
            await context.Departments
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Name == "Administration");

        if (department == null)
        {
            department = new Department
            {
                Name = "Administration",
                Description = "System Administration",
                RecordStatus = RecordStatus.Active
            };

            context.Departments.Add(department);

            await context.SaveChangesAsync();
        }

        // -------------------------
        // Position
        // -------------------------

        var position =
            await context.Positions
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x =>
                    x.Title == "System Administrator" &&
                    x.DepartmentId == department.Id);

        if (position == null)
        {
            position = new Position
            {
                Title = "System Administrator",
                JobLevel = "Senior",
                DepartmentId = department.Id,
                RecordStatus = RecordStatus.Active
            };

            context.Positions.Add(position);

            await context.SaveChangesAsync();
        }

        // -------------------------
        // Employee
        // -------------------------

        var employee =
            await context.Employees
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x =>
                    x.EmployeeCode ==
                    "EMP-0001");

        if (employee == null)
        {
            employee = new Employee
            {
                EmployeeCode = "EMP-0001",
                FirstName = "System",
                LastName = "Admin",
                Email = "admin@hrms.com",
                Phone = "01700000000",
                Address = "Head Office",
                DateOfBirth = new DateTime(1990, 1, 1),
                HireDate = DateTime.UtcNow,
                EmploymentStatus = EmploymentStatus.Permanent,
                DepartmentId = department.Id,
                PositionId = position.Id,
                RecordStatus = RecordStatus.Active
            };

            context.Employees.Add(employee);

            await context.SaveChangesAsync();
        }

        // -------------------------
        // Admin User
        // -------------------------

        var adminUser =
            await context.UserAccounts
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x =>
                    x.Username == "admin");

        if (adminUser == null)
        {
            adminUser = new UserAccount
            {
                EmployeeId = employee.Id,
                Username = "admin",
                PasswordHash = passwordService.Hash("admin123"),
                Role = UserRole.Admin,
                RecordStatus = RecordStatus.Active
            };

            context.UserAccounts.Add(adminUser);

            await context.SaveChangesAsync();
        }
    }
}
