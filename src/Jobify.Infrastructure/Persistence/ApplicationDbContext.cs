using Jobify.Domain.Enums;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Jobify.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IAuthenticatedUserService _authenticatedUserService;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IAuthenticatedUserService authenticatedUserService) :
        base(options) => _authenticatedUserService = authenticatedUserService;

    public DbSet<User> Users => Set<User>();
    public DbSet<JobApplication> JobApplications => Set<JobApplication>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<JobListing> JobListings => Set<JobListing>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UsersRoles => Set<UserRole>();
    public DbSet<Employer> Employers => Set<Employer>();
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        Guid? userId = _authenticatedUserService.Id;

        var auditLogs = new List<AuditLog>();

        var jobListingEntries = ChangeTracker.Entries<JobListing>()
            .Where(e =>
                e.State == EntityState.Added ||
                e.State == EntityState.Modified)
            .ToList();

        foreach (var entry in jobListingEntries)
        {
            var auditLog = new AuditLog
            {
                Id = Guid.NewGuid(),
                EntityType = nameof(JobListing),
                ChangedBy = userId ?? Guid.Empty,
                ChangedByType = _authenticatedUserService.Roles != null
                    ? string.Join(", ", _authenticatedUserService.Roles)
                    : null,
                ChangedAt = DateTime.UtcNow,
                EntityId = entry.Entity.Id
            };

            if (entry.State == EntityState.Added)
            {
                auditLog.Action = AuditAction.Created;
            }
            else if (entry.State == EntityState.Modified)
            {
                var status = entry.Property(nameof(JobListing.Status));

                if (status.IsModified &&
                    status.OriginalValue?.ToString() == nameof(JobStatus.Open) &&
                    status.CurrentValue?.ToString() == nameof(JobStatus.Closed))
                {
                    auditLog.Action = AuditAction.Closed;
                }
                else
                {
                    auditLog.Action = AuditAction.Updated;
                }

                auditLog.Changes = GetChanges(entry);
            }

            auditLogs.Add(auditLog);
        }

        foreach (var entry in ChangeTracker.Entries<BaseAuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = userId;
                entry.Entity.CreatedAt = DateTimeOffset.UtcNow;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.ModifiedBy = userId;
                entry.Entity.ModifiedAt = DateTimeOffset.UtcNow;
            }
        }

        if (auditLogs.Any())
        {
            AuditLogs.AddRange(auditLogs);
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<UserProfile>()
            .HasQueryFilter(x => !x.IsDeleted);

        builder.Entity<AuditLog>()
            .Property(x => x.Action)
            .HasConversion<string>()
            .IsRequired();

        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private string GetChanges(EntityEntry jobListingEntries)
    {
        var changes = new StringBuilder();

        foreach (var property in jobListingEntries.OriginalValues.Properties)
        {
            var originalValue = jobListingEntries.OriginalValues[property];
            var currentValue = jobListingEntries.CurrentValues[property];

            if (!Equals(originalValue, currentValue))
            {
                changes.AppendLine($"{property.Name}: From '{originalValue}' to '{currentValue}'");
            }
        }

        return changes.ToString();
    }
}
