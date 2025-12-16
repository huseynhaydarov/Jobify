namespace Jobify.Application.UseCases.JobApplications.Queries.GetJobApplications;

public class GetAllJobApplicationsResponse
{
    public Guid JobListingId { get; set; }
    public string? JobTitle { get; set; }

    public string? CoverLetter { get; set; }
    public ApplicationStatus ApplicationStatus { get; init; }
    public DateTimeOffset AppliedAt { get; init; }
    public DateTimeOffset? WithdrawnAt { get; init; }
}
