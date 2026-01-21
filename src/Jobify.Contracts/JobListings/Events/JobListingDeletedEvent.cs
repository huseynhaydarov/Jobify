namespace Jobify.Contracts.JobListings.Events;

public record JobListingDeletedEvent
{
    public Guid Id { get; init; }
}
