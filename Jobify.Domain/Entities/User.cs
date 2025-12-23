namespace Jobify.Domain.Entities;

public class User : BaseAuditableEntity
{
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required bool IsActive { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<JobListing> JobListings { get; set; } = new List<JobListing>();
    public ICollection<JobApplication>  Applications { get; set; } = new List<JobApplication>();

    public Employer? Employer { get; set; }
    public UserProfile? UserProfile { get; set; }
}
