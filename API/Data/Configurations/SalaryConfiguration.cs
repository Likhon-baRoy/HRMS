using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Data.Configurations;

public class SalaryConfiguration
    : IEntityTypeConfiguration<Salary>
{
    public void Configure(
        EntityTypeBuilder<Salary> builder
    )
    {
        builder.ToTable("salaries");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.BasicSalary)
            .HasPrecision(18, 2);

        builder.Property(x => x.HouseRent)
            .HasPrecision(18, 2);

        builder.Property(x => x.MedicalAllowance)
            .HasPrecision(18, 2);

        builder.Property(x => x.TransportAllowance)
            .HasPrecision(18, 2);

        builder.Property(x => x.OtherAllowance)
            .HasPrecision(18, 2);

        builder.Property(x => x.GrossSalary)
            .HasPrecision(18, 2);

        builder.Property(x => x.EffectiveDate)
            .IsRequired();

        builder.Property(x => x.IsCurrent)
            .HasDefaultValue(true);

        // Only one active salary per employee, historical rows are allowed.
        builder.HasIndex(x => x.EmployeeId)
            .IsUnique()
            .HasFilter("\"IsCurrent\" = true");

        builder.HasOne(x => x.Employee)
            .WithMany(x => x.Salaries)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
