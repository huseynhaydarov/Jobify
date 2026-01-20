using System;
using System.Collections.Generic;

namespace Jobify.Domain.Entities;

public class JobListing : BaseAuditableEntity
{
    public Guid EmployerId { get; set; }
    public Employer? Employer { get; set; }

    public Guid CompanyId { get; set; }
    public Company? Company { get; set; }

    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Requirements { get; set; }
    public string? Location { get; set; }
    public decimal? Salary { get; set; }
    public string? Currency { get; set; }
    public required JobStatus Status { get; set; } = JobStatus.Open;
    public required DateTimeOffset PostedAt { get; set; }
    public DateTimeOffset? ExpiresAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? ClosedAt { get; set; }

    public ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();
}
