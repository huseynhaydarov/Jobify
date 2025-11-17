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
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UsersRoles => Set<UserRole>();
    public DbSet<Employer> Employers => Set<Employer>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Ignore<BaseAuditableEntity>();
        builder.Ignore<BaseEntity>();

        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<BaseAuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTimeOffset.UtcNow;;
                    entry.Entity.CreatedBy = _authenticatedUser.Id;
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifiedAt = DateTimeOffset.UtcNow;
                    entry.Entity.ModifiedBy = _authenticatedUser.Id;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

}
