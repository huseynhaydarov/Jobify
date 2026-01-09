namespace Jobify.Infrastructure.Persistence.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.Property(u => u.Email)
            .HasMaxLength(155)
            .IsRequired();

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.PasswordHash)
            .HasMaxLength(255);

        builder.Property(u => u.IsActive)
            .HasDefaultValue(false);

        builder.HasMany(u => u.UserRoles)
            .WithOne(u => u.User)
            .HasForeignKey(u => u.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.Applications)
            .WithOne(u => u.User)
            .HasForeignKey(u => u.ApplicantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(u => u.Employer)
            .WithOne(m => m.User)
            .HasForeignKey<Employer>(u => u.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.UserProfile)
            .WithOne(x => x.User)
            .HasForeignKey<UserProfile>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
