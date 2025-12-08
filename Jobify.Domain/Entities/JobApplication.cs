namespace Jobify.Domain.Entities;

public class JobApplication : BaseAuditableEntity
{
    public required Guid  JobListingId { get; set; }
    public JobListing? JobListing { get; set; }

    public required Guid ApplicantId { get; set; }
    public User? User { get; set; }

    public string? CoverLetter { get; set; }
    public required ApplicationStatus ApplicationStatus { get; set; } = ApplicationStatus.Applied;
    public required DateTimeOffset AppliedAt { get; set; }
    public DateTime? WithdrawnAt { get; set; }
}
