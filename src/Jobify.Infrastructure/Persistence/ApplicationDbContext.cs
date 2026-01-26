using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Jobify.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IAuthenticatedUserService _authenticatedUserService;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
        IAuthenticatedUserService authenticatedUserService) :
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

    public new EntityEntry Entry(object entity)
        => base.Entry(entity);

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var userId = _authenticatedUserService.Id;

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

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<UserProfile>()
            .HasQueryFilter(x => !x.IsDeleted);

        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
