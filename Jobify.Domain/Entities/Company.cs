using Jobify.Domain.Common.BaseEntities;

namespace Jobify.Domain.Entities;

public class Company : BaseAuditableEntity
{
    public required string Name { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? Description { get; set; }
    public string? Industry { get; set; }

    public new required Guid CreatedById { get; set; }
    public User? User { get; set; }

    public ICollection<JobListing> JobListings { get; set; } = new List<JobListing>();
}
