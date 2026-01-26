namespace Jobify.Infrastructure.Persistence.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLogs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.EntityType)
            .IsRequired();

        builder.Property(x => x.Action)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.ChangedBy)
            .IsRequired();

        builder.Property(x => x.ChangedByType);

        builder.Property(x => x.AuditLogDetails)
            .HasColumnType("jsonb")
            .HasDefaultValueSql("'[]'::jsonb");

        builder.Property(x => x.EntityId)
            .IsRequired();
    }
}
