namespace Jobify.Infrastructure.Persistence.Configurations;

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
    }
}
