namespace Jobify.Infrastructure.Persistence.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<JobApplication> JobApplications =>  Set<JobApplication>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<JobListing> JobListings => Set<JobListing>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UsersRoles => Set<UserRole>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Ignore<BaseAuditableEntity>();
        builder.Ignore<BaseEntity>();

        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTimeOffset.Now;;
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifiedAt = DateTimeOffset.Now;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

}
