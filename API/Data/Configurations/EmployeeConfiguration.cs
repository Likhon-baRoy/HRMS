using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Data.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("employees");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.EmployeeCode)
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(x => x.EmployeeCode)
            .IsUnique();

        builder.Property(x => x.FirstName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.LastName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.HasIndex(x => new
            {
                x.Phone,
                x.RecordStatus
            })
            .IsUnique();

        builder.Property(x => x.Phone)
            .HasMaxLength(11)
            .IsRequired();

        builder.HasIndex(x => x.Phone)
            .IsUnique();

        builder.Property(x => x.AccountNumber)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(x => x.EmploymentStatus)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.RecordStatus)
            .HasConversion<int>()
            .IsRequired();

        // Department relationship
        builder.HasOne(x => x.Department)
            .WithMany(x => x.Employees)
            .HasForeignKey(x => x.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Position relationship
        builder.HasOne(x => x.Position)
            .WithMany(x => x.Employees)
            .HasForeignKey(x => x.PositionId)
            .OnDelete(DeleteBehavior.Restrict);

        // One-to-one UserAccount
        builder.HasOne(x => x.UserAccount)
            .WithOne(x => x.Employee)
            .HasForeignKey<UserAccount>(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
