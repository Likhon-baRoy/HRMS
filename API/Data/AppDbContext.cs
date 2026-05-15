using API.Models;
using API.Models.Enums;
using API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserService currentUser) : DbContext(options)
{
    public DbSet<Employee> Employees { get; set; }

    public DbSet<UserAccount> UserAccounts { get; set; }

    public DbSet<Department> Departments { get; set; }

    public DbSet<Position> Positions { get; set; }

    public DbSet<Attendance> Attendances { get; set; }

    public DbSet<Salary> Salaries { get; set; }

    public DbSet<SalaryRevision> SalaryRevisions { get; set; }

    public DbSet<Payroll> Payrolls { get; set; }

    public DbSet<Bonus> Bonuses { get; set; }

    public DbSet<Deduction> Deductions { get; set; }

    public DbSet<PayrollDeduction> PayrollDeductions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder.Entity<Employee>()
            .HasQueryFilter(x =>
                x.EmployeeStatus != EmployeeStatus.Resigned &&
                x.EmployeeStatus != EmployeeStatus.Terminated);

        modelBuilder.Entity<Department>()
            .HasQueryFilter(x => x.RecordStatus == RecordStatus.Active);

        modelBuilder.Entity<Position>()
            .HasQueryFilter(x => x.RecordStatus == RecordStatus.Active);

        modelBuilder.Entity<UserAccount>()
            .HasQueryFilter(x => x.RecordStatus == RecordStatus.Active);
    }

    public override int SaveChanges()
    {
        ApplyAuditInfo();

        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditInfo();

        return await base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAuditInfo()
    {
        var entries = ChangeTracker
            .Entries<BaseEntity>();

        var now = DateTime.UtcNow;

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:

                    entry.Entity.CreatedAt = now;
                    entry.Entity.CreatedBy = currentUser.UserId;

                    break;

                case EntityState.Modified:

                    entry.Property(x => x.CreatedAt)
                        .IsModified = false; // never overwrite the original creation timestamp
                    entry.Property(x => x.CreatedBy)
                        .IsModified = false;

                    entry.Entity.UpdatedAt = now;
                    entry.Entity.UpdatedBy = currentUser.UserId;

                    break;
            }
        }
    }
}
