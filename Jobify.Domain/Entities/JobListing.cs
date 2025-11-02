using Jobify.Domain.Common.BaseEntities;
using Jobify.Domain.Enums;

namespace Jobify.Domain.Entities;

public class JobListing : BaseAuditableEntity
{
    public Guid? EmployerId { get; set; }
    public User? User { get; set; }

    public Guid? CompanyId { get; set; }
    public Company? Company { get; set; }

    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Requirements { get; set; }
    public string? Location { get; set; }
    public decimal? Salary { get; set; }
    public string? Currency { get; set; }
    public required JobStatus Status { get; set; } = JobStatus.Open;
    public required DateTime PostedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public int Views { get; set; }

    public ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();
    public ICollection<Message> Messages { get; set; } = new List<Message>();
}
