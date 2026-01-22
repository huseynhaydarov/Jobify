namespace Jobify.Contracts.JobListings.Events;

public record JobListingCreatedEvent
{
    public Guid Id { get; init; }

    public required string Name { get; init; }
    public string? Description { get; init; }
    public string? Requirements { get; init; }
    public string? Location { get; init; }

    public decimal? Salary { get; init; }
    public string? Currency { get; init; } = default!;

    public string Status { get; init; } = default!;

    public DateTimeOffset PostedAt { get; init; }
    public DateTimeOffset? ExpiresAt { get; init; }
    public Guid CompanyId { get; init; }
    public Guid EmployerId { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public Guid? CreatedById { get; init; }
    public string? CreatedBy {get; init;}
}
