using Jobify.Domain.Common.BaseEntities;
using Jobify.Domain.Common.Enumerations;

namespace Jobify.Domain.Common.Entities;

public class JobApplication : BaseAuditableEntity
{
    public required Guid  JobListingId { get; set; }
    public JobListing? JobListing { get; set; }

    public required Guid ApplicantId { get; set; }
    public User? User { get; set; }

    public string? CoverLetter { get; set; }
    public required ApplicationStatus ApplicationStatus { get; set; } = ApplicationStatus.Applied;
    public required DateTime AppliedAt { get; set; } = DateTime.Now;
    public DateTime? WithdrawnAt { get; set; }
}
