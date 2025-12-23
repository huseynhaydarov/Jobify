namespace Jobify.Application.UseCases.JobListings.Commands.UpdateJobListing;

public record UpdateJobListingCommand(
    Guid Id,
    Guid CompanyId,
    string Name,
    string? Description,
    string? Requirements,
    string? Location,
    decimal? Salary,
    string? Currency,
    DateTimeOffset? ExpireDate) : IRequest<Unit>;
