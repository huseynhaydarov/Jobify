namespace Jobify.Infrastructure.Persistence.Data.Configurations;

public class EmployerConfiguration : IEntityTypeConfiguration<Employer>
{
    public void Configure(EntityTypeBuilder<Employer> builder)
    {
        builder.ToTable("Employers");

        builder.HasKey(e => e.Id);

        builder.HasOne(e => e.User)
            .WithMany(u => u.Employers)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Company)
            .WithMany(c => c.Employers)
            .HasForeignKey(e => e.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(e => e.Position)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.JoinedAt)
            .IsRequired();
    }
}
