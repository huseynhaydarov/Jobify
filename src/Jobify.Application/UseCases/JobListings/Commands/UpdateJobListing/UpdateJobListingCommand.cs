namespace Jobify.Application.UseCases.JobListings.Commands.UpdateJobListing;

public record UpdateJobListingCommand(
    Guid Id,
    string Name,
    string? Description,
    string? Requirements,
    string? Location,
    decimal? Salary,
    JobStatus Status,
    string? Currency,
    DateTimeOffset? ExpireDate) : IRequest<UpdateJobListingResponse>;
