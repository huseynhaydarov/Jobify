using System.Reflection;
using Jobify.Application.Common.Interfaces.Data;
using Jobify.Domain.Common.BaseEntities;
using Jobify.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Jobify.Infrastructure.Persistence.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<JobApplication> JobApplications { get; }
    public DbSet<Company> Companies { get; }
    public DbSet<JobListing> JobListings { get; }
    public DbSet<Message> Messages { get; }
    public DbSet<Role> Roles { get; }
    public DbSet<UserRole> UsersRoles { get; }

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
                    entry.Entity.CreatedAt = DateTimeOffset.UtcNow;;
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifiedAt = DateTime.Now;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

}
