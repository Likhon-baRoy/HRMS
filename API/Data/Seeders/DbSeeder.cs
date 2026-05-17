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
        // CRITICAL FIX: Check Departments instead of UserAccounts to prevent
        // duplicate key violations if a previous run crashed halfway.
        var alreadySeeded = await context.Departments
            .IgnoreQueryFilters()
            .AnyAsync();

        if (alreadySeeded)
        {
            await EnsureDemoAccountsAsync(context);
            return;
        }

        var passwordService = context.GetService<IPasswordService>();
        var utcNow = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
        const int systemUserId = 1; // Default fallback system user ID for seeder

        // =====================
        // Departments
        // =====================

        var departments = new List<Department>
        {
            new()
            {
                Name = "Administration",
                Description = "System Administration",
                RecordStatus = RecordStatus.Active,
                CreatedAt = utcNow,
                CreatedBy = systemUserId
            },
            new()
            {
                Name = "Human Resource",
                Description = "HR Department",
                RecordStatus = RecordStatus.Active,
                CreatedAt = utcNow,
                CreatedBy = systemUserId
            },
            new()
            {
                Name = "Accounts",
                Description = "Finance Department",
                RecordStatus = RecordStatus.Active,
                CreatedAt = utcNow,
                CreatedBy = systemUserId
            },
            new()
            {
                Name = "IT",
                Description = "Technology Department",
                RecordStatus = RecordStatus.Active,
                CreatedAt = utcNow,
                CreatedBy = systemUserId
            },
            new()
            {
                Name = "Operations",
                Description = "Operations Team",
                RecordStatus = RecordStatus.Active,
                CreatedAt = utcNow,
                CreatedBy = systemUserId
            }
        };

        context.Departments.AddRange(departments);
        await context.SaveChangesAsync();

        // =====================
        // Positions
        // =====================

        var positions = new List<Position>
        {
            new()
            {
                Title = "System Administrator",
                JobLevel = "Senior",
                DepartmentId = departments[0].Id,
                RecordStatus = RecordStatus.Active,
                CreatedAt = utcNow,
                CreatedBy = systemUserId
            },
            new()
            {
                Title = "HR Manager",
                JobLevel = "Manager",
                DepartmentId = departments[1].Id,
                RecordStatus = RecordStatus.Active,
                CreatedAt = utcNow,
                CreatedBy = systemUserId
            },
            new()
            {
                Title = "Accountant",
                JobLevel = "Mid",
                DepartmentId = departments[2].Id,
                RecordStatus = RecordStatus.Active,
                CreatedAt = utcNow,
                CreatedBy = systemUserId
            },
            new()
            {
                Title = "Software Engineer",
                JobLevel = "Mid",
                DepartmentId = departments[3].Id,
                RecordStatus = RecordStatus.Active,
                CreatedAt = utcNow,
                CreatedBy = systemUserId
            },
            new()
            {
                Title = "Operations Executive",
                JobLevel = "Junior",
                DepartmentId = departments[4].Id,
                RecordStatus = RecordStatus.Active,
                CreatedAt = utcNow,
                CreatedBy = systemUserId
            }
        };

        context.Positions.AddRange(positions);
        await context.SaveChangesAsync();

        // =====================
        // Employees
        // =====================

        var employees = new List<Employee>
        {
            new()
            {
                EmployeeCode = "EMP-0001",
                FirstName = "System",
                LastName = "Admin",
                Email = "admin@hrms.com",
                Phone = "01700000001",
                Address = "Head Office",
                DateOfBirth = DateTime.SpecifyKind(new DateTime(1990, 1, 1), DateTimeKind.Utc),
                HireDate = utcNow,
                EmploymentType = EmploymentType.Permanent,
                EmployeeStatus = EmployeeStatus.Active,
                DepartmentId = departments[0].Id,
                PositionId = positions[0].Id,
                CreatedAt = utcNow,
                CreatedBy = systemUserId
            },
            new()
            {
                EmployeeCode = "EMP-0002",
                FirstName = "Sarah",
                LastName = "Khan",
                Email = "hr@hrms.com",
                Phone = "01700000002",
                Address = "Dhaka",
                DateOfBirth = DateTime.SpecifyKind(new DateTime(1992, 5, 15), DateTimeKind.Utc),
                HireDate = utcNow,
                EmploymentType = EmploymentType.Permanent,
                EmployeeStatus = EmployeeStatus.Active,
                DepartmentId = departments[1].Id,
                PositionId = positions[1].Id,
                CreatedAt = utcNow,
                CreatedBy = systemUserId
            },
            new()
            {
                EmployeeCode = "EMP-0003",
                FirstName = "John",
                LastName = "Doe",
                Email = "manager@hrms.com",
                Phone = "01700000003",
                Address = "Khulna",
                DateOfBirth = DateTime.SpecifyKind(new DateTime(1994, 2, 10), DateTimeKind.Utc),
                HireDate = utcNow,
                EmploymentType = EmploymentType.Permanent,
                EmployeeStatus = EmployeeStatus.Active,
                DepartmentId = departments[3].Id,
                PositionId = positions[3].Id,
                CreatedAt = utcNow,
                CreatedBy = systemUserId
            },
            new()
            {
                EmployeeCode = "EMP-0004",
                FirstName = "Employee",
                LastName = "User",
                Email = "employee@hrms.com",
                Phone = "01700000004",
                Address = "Chittagong",
                DateOfBirth = DateTime.SpecifyKind(new DateTime(1998, 8, 20), DateTimeKind.Utc),
                HireDate = utcNow,
                EmploymentType = EmploymentType.Probation,
                EmployeeStatus = EmployeeStatus.Active,
                DepartmentId = departments[4].Id,
                PositionId = positions[4].Id,
                CreatedAt = utcNow,
                CreatedBy = systemUserId
            }
        };

        context.Employees.AddRange(employees);
        await context.SaveChangesAsync();

        // =====================
        // User Accounts
        // =====================

        var users = new List<UserAccount>
        {
            new()
            {
                EmployeeId = employees[0].Id,
                Username = "admin",
                PasswordHash = passwordService.Hash("Admin@123"),
                Role = UserRole.Admin,
                RecordStatus = RecordStatus.Active,
                CreatedAt = utcNow,
                CreatedBy = systemUserId
            },
            new()
            {
                EmployeeId = employees[1].Id,
                Username = "hr",
                PasswordHash = passwordService.Hash("Hr@123"),
                Role = UserRole.Hr,
                RecordStatus = RecordStatus.Active,
                CreatedAt = utcNow,
                CreatedBy = systemUserId
            },
            new()
            {
                EmployeeId = employees[2].Id,
                Username = "manager",
                PasswordHash = passwordService.Hash("Manager@123"),
                Role = UserRole.Manager,
                RecordStatus = RecordStatus.Active,
                CreatedAt = utcNow,
                CreatedBy = systemUserId
            },
            new()
            {
                EmployeeId = employees[3].Id,
                Username = "employee",
                PasswordHash = passwordService.Hash("Employee@123"),
                Role = UserRole.Employee,
                RecordStatus = RecordStatus.Active,
                CreatedAt = utcNow,
                CreatedBy = systemUserId
            }
        };

        context.UserAccounts.AddRange(users);
        await context.SaveChangesAsync();
    }

    private static async Task EnsureDemoAccountsAsync(AppDbContext context)
    {
        var passwordService = context.GetService<IPasswordService>();
        var utcNow = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
        const int systemUserId = 1;

        var departments = await context.Departments
            .IgnoreQueryFilters()
            .ToListAsync();

        var positions = await context.Positions
            .IgnoreQueryFilters()
            .ToListAsync();

        if (departments.Count == 0 || positions.Count == 0)
            return;

        var admin = await EnsureDemoEmployeeAsync(
            context,
            departments,
            positions,
            "EMP-0001",
            "System",
            "Admin",
            "admin@hrms.com",
            "01700000001",
            "Head Office",
            "Administration",
            "System Administrator",
            EmploymentType.Permanent,
            utcNow,
            systemUserId);

        var hr = await EnsureDemoEmployeeAsync(
            context,
            departments,
            positions,
            "EMP-0002",
            "Sarah",
            "Khan",
            "hr@hrms.com",
            "01700000002",
            "Dhaka",
            "Human Resource",
            "HR Manager",
            EmploymentType.Permanent,
            utcNow,
            systemUserId);

        var manager = await EnsureDemoEmployeeAsync(
            context,
            departments,
            positions,
            "EMP-0003",
            "John",
            "Doe",
            "manager@hrms.com",
            "01700000003",
            "Khulna",
            "Accounts",
            "Accountant",
            EmploymentType.Permanent,
            utcNow,
            systemUserId);

        var employee = await EnsureDemoEmployeeAsync(
            context,
            departments,
            positions,
            "EMP-0004",
            "Employee",
            "User",
            "employee@hrms.com",
            "01700000004",
            "Chittagong",
            "Operations",
            "Operations Executive",
            EmploymentType.Probation,
            utcNow,
            systemUserId);

        await EnsureDemoUserAsync(
            context,
            passwordService,
            admin.Id,
            "admin",
            "Admin@123",
            UserRole.Admin,
            utcNow,
            systemUserId);

        await EnsureDemoUserAsync(
            context,
            passwordService,
            hr.Id,
            "hr",
            "Hr@123",
            UserRole.Hr,
            utcNow,
            systemUserId);

        await EnsureDemoUserAsync(
            context,
            passwordService,
            manager.Id,
            "manager",
            "Manager@123",
            UserRole.Manager,
            utcNow,
            systemUserId);

        await EnsureDemoUserAsync(
            context,
            passwordService,
            employee.Id,
            "employee",
            "Employee@123",
            UserRole.Employee,
            utcNow,
            systemUserId);

        await context.SaveChangesAsync();
    }

    private static async Task<Employee> EnsureDemoEmployeeAsync(
        AppDbContext context,
        List<Department> departments,
        List<Position> positions,
        string employeeCode,
        string firstName,
        string lastName,
        string email,
        string phone,
        string address,
        string departmentName,
        string positionTitle,
        EmploymentType employmentType,
        DateTime utcNow,
        int systemUserId)
    {
        var employee = await context.Employees
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x =>
                x.EmployeeCode == employeeCode ||
                x.Email == email);

        var department = departments
            .FirstOrDefault(x => x.Name == departmentName)
            ?? departments[0];

        var position = positions
            .FirstOrDefault(x => x.Title == positionTitle)
            ?? positions[0];

        if (employee == null)
        {
            employee = new Employee
            {
                EmployeeCode = employeeCode,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Phone = phone,
                Address = address,
                DateOfBirth = DateTime.SpecifyKind(new DateTime(1995, 1, 1), DateTimeKind.Utc),
                HireDate = utcNow,
                EmploymentType = employmentType,
                EmployeeStatus = EmployeeStatus.Active,
                DepartmentId = department.Id,
                PositionId = position.Id,
                CreatedAt = utcNow,
                CreatedBy = systemUserId
            };

            context.Employees.Add(employee);
            await context.SaveChangesAsync();

            return employee;
        }

        employee.FirstName = firstName;
        employee.LastName = lastName;
        employee.EmployeeStatus = EmployeeStatus.Active;
        employee.DepartmentId = department.Id;
        employee.PositionId = position.Id;
        employee.UpdatedAt = utcNow;
        employee.UpdatedBy = systemUserId;

        return employee;
    }

    private static async Task EnsureDemoUserAsync(
        AppDbContext context,
        IPasswordService passwordService,
        int employeeId,
        string username,
        string password,
        UserRole role,
        DateTime utcNow,
        int systemUserId)
    {
        var user = await context.UserAccounts
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Username == username);

        if (user == null)
        {
            context.UserAccounts.Add(new UserAccount
            {
                EmployeeId = employeeId,
                Username = username,
                PasswordHash = passwordService.Hash(password),
                Role = role,
                RecordStatus = RecordStatus.Active,
                CreatedAt = utcNow,
                CreatedBy = systemUserId
            });

            return;
        }

        user.EmployeeId = employeeId;
        user.PasswordHash = passwordService.Hash(password);
        user.Role = role;
        user.RecordStatus = RecordStatus.Active;
        user.UpdatedAt = utcNow;
        user.UpdatedBy = systemUserId;
    }
}
