using Jobify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jobify.Infrastructure.Persistence.Data.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Companies");

        builder.Property(c => c.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.WebsiteUrl)
            .HasMaxLength(150);

        builder.Property(c => c.Description)
            .HasMaxLength(500);

        builder.Property(c => c.Industry)
            .HasMaxLength(100);

        builder
            .HasOne(c => c.User)
            .WithMany(u => u.Companies)
            .HasForeignKey(c => c.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.JobListings)
            .WithOne(j => j.Company)
            .HasForeignKey(j => j.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
