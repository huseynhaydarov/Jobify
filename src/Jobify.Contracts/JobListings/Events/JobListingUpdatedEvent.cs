using Jobify.Domain.Entities;

namespace Jobify.Contracts.JobListings.Events;

public record JobListingUpdatedEvent
{
   public Guid Id { get; init; }
   public required string Name { get; init; }
   public string? Description  { get; init; }
   public string? Requirements { get; init; }
   public string? Location  { get; init; }
   public decimal? Salary   { get; init; }
   public string Status { get; init; } = default!;
   public string? Currency   { get; init; }
   public DateTimeOffset? ExpireDate { get; init; }
   public DateTimeOffset ModifiedAt { get; init; }
   public Guid ModifiedById { get; init; }
   public string? ModifiedBy {get; init;}
   public List<AuditLogDetail> Changes { get; set; } = [];
}
