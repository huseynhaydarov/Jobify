namespace Jobify.Contracts.JobListings.Events;

public record JobListingDeletedEvent
{
    public Guid Id { get; init; }
    public DateTimeOffset ClosedAt { get; init; }
    public Guid ClosedById { get; init; }
    public string? ClosedBy { get; init; }
}
