using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Jobify.Application.Common.Interfaces.Data;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<JobApplication> JobApplications { get; }
    DbSet<Company> Companies { get; }
    DbSet<JobListing> JobListings { get; }
    DbSet<Role> Roles { get; }
    DbSet<UserRole> UsersRoles { get; }
    DbSet<Employer> Employers { get; }
    DbSet<UserProfile> UserProfiles { get; }
    DbSet<AuditLog> AuditLogs { get; }

    EntityEntry Entry(object entity);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
