using Jobify.Contracts.Common;

namespace Jobify.Contracts.JobListings.IntegrationEvents;

public record JobListingCreated : IntegrationEvent
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public string? Requirements { get; init; }
    public string? Location { get; init; }
    public DateTimeOffset PostedAt { get; init; }
}
