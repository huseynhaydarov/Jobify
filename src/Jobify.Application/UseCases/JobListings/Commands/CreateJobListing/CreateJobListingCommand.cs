namespace Jobify.Application.UseCases.JobListings.Commands.CreateJobListing;

public record CreateJobListingCommand(
    string Name,
    string? Description,
    string? Requirements,
    string? Location,
    decimal? Salary,
    string? Currency,
    DateTimeOffset? ExpireDate) : IRequest<JobListingDto>;
