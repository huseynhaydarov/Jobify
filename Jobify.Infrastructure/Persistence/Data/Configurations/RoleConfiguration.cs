using Jobify.Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jobify.Infrastructure.Persistence.Data.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Description)
            .HasMaxLength(50);

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.HasMany(m => m.UserRoles)
            .WithOne(m => m.Role)
            .HasForeignKey(m => m.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(
            new Role
            {
                Id = Guid.Parse("cdb9e288-36c7-4ae0-b517-476d9cd0224b"),
                Name = "Administrator",
                Description = "Administrator of the system",
                IsActive = true,
                CreatedAt = new DateTime(2025, 10, 28, 12, 0, 0)
            },
            new Role
            {
                Id = Guid.Parse("7db11503-a756-4e92-872f-d18c0aa963b2"),
                Name = "Guest",
                Description = "Guest of the system",
                IsActive = true,
                CreatedAt = new DateTime(2025, 10, 28, 12, 0, 0)
            },
            new Role
            {
                Id = Guid.Parse("bb176f73-41a2-4b9d-b85c-3805e8d8ee12"),
                Name = "Employer",
                Description = "Employer of the system",
                IsActive = true,
                CreatedAt = new DateTime(2025, 10, 28, 12, 0, 0)
            },
            new Role
            {
                Id = Guid.Parse("2a9abd5b-36c2-4dad-abac-953b6b4b03be"),
                Name = "Job Seeker",
                Description = "Guest of the System",
                IsActive = true,
                CreatedAt = new DateTime(2025, 10, 28, 12, 0, 0)
            }
        );
    }
}
