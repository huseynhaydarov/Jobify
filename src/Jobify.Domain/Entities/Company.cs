namespace Jobify.Domain.Entities;

public class Company : BaseAuditableEntity
{
    public required string Name { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? Description { get; set; }
    public string? Industry { get; set; }

    public bool IsDeleted { get; set; }

    public required Guid CreatedById { get; set; }
    public User? User { get; set; }
    public ICollection<Employer> Employers { get; set; } = new List<Employer>();
    public ICollection<JobListing> JobListings { get; set; } = new List<JobListing>();
}
