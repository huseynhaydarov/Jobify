namespace Jobify.Application.UseCases.JobApplications.Queries.GetJobApplicationDetail;

public class GetJobApplicationDetailResponse
{
    public Guid Id { get; set; }

    public Guid JobListingId { get; set; }
    public string JobTitle { get; set; }
    public string CompanyName { get; set; }

    public Guid ApplicantId { get; set; }
    public string ApplicantName { get; set; } = default!;
    public string ApplicantEmail { get; set; } = default!;

    public string? CoverLetter { get; set; }
    public ApplicationStatus ApplicationStatus { get; set; }
    public DateTimeOffset AppliedAt { get; set; }
    public DateTimeOffset? WithdrawnAt { get; set; }
}
