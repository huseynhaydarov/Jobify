using System;
using Jobify.Domain.Enums;

namespace Jobify.Application.UseCases.JobApplications.Queries.GetJobApplications;

public class GetAllJobApplicationsByJobListingResponse
{
    public Guid Id { get; set; }
    public string? JobTitle { get; set; }

    public string? CoverLetter { get; set; }
    public ApplicationStatus ApplicationStatus { get; init; }
    public DateTimeOffset AppliedAt { get; init; }
    public DateTimeOffset? WithdrawnAt { get; init; }
}
