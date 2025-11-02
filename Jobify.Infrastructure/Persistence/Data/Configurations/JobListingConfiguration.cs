using Jobify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jobify.Infrastructure.Persistence.Data.Configurations;

public class JobListingConfiguration : IEntityTypeConfiguration<JobListing>
{
    public void Configure(EntityTypeBuilder<JobListing> builder)
    {
        builder.ToTable("JobListings");

        builder.Property(j => j.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(j => j.Description)
            .HasMaxLength(500);

        builder.Property(j => j.Requirements)
            .HasMaxLength(500);

        builder.Property(j => j.Location)
            .HasMaxLength(100);

        builder.Property(j => j.Salary)
            .HasPrecision(14, 2)
            .IsRequired();

        builder.Property(j => j.Currency)
            .HasMaxLength(3);

        builder.Property(j => j.Status)
            .IsRequired();

        builder.Property(j => j.PostedAt);

        builder.Property(j => j.ExpiresAt);

        builder.Property(j => j.Views);

        builder.HasOne(j => j.Company)
            .WithMany(c => c.JobListings)
            .HasForeignKey(j => j.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(j => j.User)
            .WithMany(u => u.JobListings)
            .HasForeignKey(j => j.EmployerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(m => m.Applications)
            .WithOne(j => j.JobListing)
            .HasForeignKey(j => j.JobListingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(m => m.Messages)
            .WithOne(j => j.JobListing)
            .HasForeignKey(j => j.JobId)
            .OnDelete(DeleteBehavior.Cascade);;
    }
}
