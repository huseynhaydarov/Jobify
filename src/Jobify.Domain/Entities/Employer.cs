namespace Jobify.Domain.Entities;

public class Employer : BaseAuditableEntity
{
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public Guid CompanyId { get; set; }
    public Company? Company { get; set; }

    public EmployerPosition? Position { get; set; }

    public DateTimeOffset JoinedAt { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<JobListing> JobListings { get; set; } = new List<JobListing>();
}
