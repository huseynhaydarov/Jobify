using Jobify.Contracts.Common;

namespace Jobify.Contracts.JobListings.IntegrationEvents;

public record JobListingUpdated : IntegrationEvent
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string? Description { get; init; }
    public string? Requirements { get; init; }
    public string? Location { get; init; }
    public decimal? Salary { get; init; }
    public string Status { get; init; }
    public string? Currency { get; init; }
}
