using Jobify.Domain.Common.BaseEntities;

namespace Jobify.Domain.Entities;

public class User : BaseAuditableEntity
{
    public required string  Username { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required bool IsActive { get; set; }

    public Guid IdentityUserId { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<JobListing> JobListings { get; set; } = new List<JobListing>();
    public ICollection<JobApplication>  Applications { get; set; } = new List<JobApplication>();
    public ICollection<Message> SentMessages { get; set; } = new List<Message>();
    public ICollection<Message> ReceivedMessages  { get; set; } = new List<Message>();

    public ICollection<Company> Companies { get; set; } = new List<Company>();
}
