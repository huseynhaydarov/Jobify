using Jobify.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jobify.Infrastructure.Persistence.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("DomainUsers");

        builder.HasMany<IdentityUserRole<Guid>>()
            .WithOne()
            .HasForeignKey(u => u.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.JobListings)
            .WithOne()
            .HasForeignKey(u => u.EmployerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.Applications)
            .WithOne()
            .HasForeignKey(u => u.ApplicantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.SentMessages)
            .WithOne()
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.ReceivedMessages)
            .WithOne()
            .HasForeignKey(m => m.ReceiverId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Companies)
            .WithOne()
            .HasForeignKey(c => c.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
