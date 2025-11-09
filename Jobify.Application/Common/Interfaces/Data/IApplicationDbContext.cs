namespace Jobify.Application.Common.Interfaces.Data;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<JobApplication> JobApplications { get; }
    DbSet<Company> Companies { get; }
    DbSet<JobListing> JobListings { get; }
    DbSet<Message> Messages { get; }
    DbSet<Role> Roles { get; }
    DbSet<UserRole> UsersRoles { get; }
    DbSet<Employer> Employers { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
