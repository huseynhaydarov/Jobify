namespace Jobify.Application.UseCases.JobApplications.Commands.CreateJobApplication;

public record CreateJobApplicationCommand(
    Guid JobListingId,
    string? CoverLetter) : IRequest<JobApplicationDto>;
