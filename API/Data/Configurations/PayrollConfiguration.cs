using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Data.Configurations;

public class PayrollConfiguration : IEntityTypeConfiguration<Payroll>
{
    public void Configure(EntityTypeBuilder<Payroll> builder)
    {
        builder.ToTable("payrolls");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.GrossSalary)
            .HasPrecision(18, 2);

        builder.Property(x => x.TotalBonus)
            .HasPrecision(18, 2);

        builder.Property(x => x.TotalDeductions)
            .HasPrecision(18, 2);

        builder.Property(x => x.TaxAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.NetSalary)
            .HasPrecision(18, 2);

        builder.Property(x => x.Status)
            .HasConversion<int>();

        builder.HasOne(x => x.Employee)
            .WithMany(x => x.Payrolls)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
