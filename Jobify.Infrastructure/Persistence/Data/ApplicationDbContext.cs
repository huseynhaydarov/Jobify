namespace Jobify.Infrastructure.Persistence.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IAuthenticatedUser _authenticatedUser;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IAuthenticatedUser authenticatedUser) : base(options)
    {
        _authenticatedUser = authenticatedUser;
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<JobApplication> JobApplications =>  Set<JobApplication>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<JobListing> JobListings => Set<JobListing>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UsersRoles => Set<UserRole>();
    public DbSet<Employer> Employers => Set<Employer>();
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<UserProfile>()
            .HasQueryFilter(x => !x.IsDeleted);

        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var userId = _authenticatedUser.Id;

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
}
