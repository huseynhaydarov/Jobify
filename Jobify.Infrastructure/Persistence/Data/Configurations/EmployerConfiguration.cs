namespace Jobify.Infrastructure.Persistence.Data.Configurations;

public class EmployerConfiguration : IEntityTypeConfiguration<Employer>
{
    public void Configure(EntityTypeBuilder<Employer> builder)
    {
        builder.ToTable("Employers");

        builder.HasKey(e => e.Id);

        builder.HasMany(m => m.JobListings)
            .WithOne(j => j.Employer)
            .HasForeignKey(j => j.EmployerId)
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
