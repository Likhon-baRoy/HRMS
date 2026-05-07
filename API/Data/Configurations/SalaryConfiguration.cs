using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Data.Configurations;

public class SalaryConfiguration : IEntityTypeConfiguration<Salary>
{
    public void Configure(EntityTypeBuilder<Salary> builder)
    {
        builder.ToTable("salaries");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.BaseSalary)
            .HasPrecision(18, 2);

        builder.Property(x => x.Allowance)
            .HasPrecision(18, 2);

        builder.HasOne(x => x.Employee)
            .WithMany(x => x.Salaries)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
