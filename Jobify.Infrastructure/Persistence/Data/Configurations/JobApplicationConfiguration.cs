namespace Jobify.Infrastructure.Persistence.Data.Configurations;

public class JobApplicationConfiguration : IEntityTypeConfiguration<JobApplication>
{
    public void Configure(EntityTypeBuilder<JobApplication> builder)
    {
        builder.ToTable("JobApplications");

        builder.Property(a => a.CoverLetter)
            .HasMaxLength(500);

        builder.Property(a => a.ApplicationStatus)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(a => a.AppliedAt)
            .IsRequired();

        builder.Property(a => a.WithdrawnAt);

        builder.HasOne(a => a.JobListing)
            .WithMany(a => a.Applications)
            .HasForeignKey(a => a.JobListingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.User)
            .WithMany(a => a.Applications)
            .HasForeignKey(a => a.ApplicantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
