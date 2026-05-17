using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Data.Configurations;

public class SalaryRevisionConfiguration : IEntityTypeConfiguration<SalaryRevision>
{
    public void Configure(EntityTypeBuilder<SalaryRevision> builder)
    {
        builder.ToTable("SalaryRevisions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.OldGrossSalary)
            .HasPrecision(18, 2);

        builder.Property(x => x.NewGrossSalary)
            .HasPrecision(18, 2);

        builder.Property(x => x.Reason)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.RevisionDate)
            .IsRequired();

        builder.HasOne(x => x.Salary)
            .WithMany()
            .HasForeignKey(x => x.SalaryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.SalaryId);
    }
}
