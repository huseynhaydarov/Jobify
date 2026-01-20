using Jobify.Contracts.Common;

namespace Jobify.Contracts.JobListings.IntegrationEvents;

public record JobListingDeleted : IntegrationEvent
{
    public Guid Id { get; init; }
}
