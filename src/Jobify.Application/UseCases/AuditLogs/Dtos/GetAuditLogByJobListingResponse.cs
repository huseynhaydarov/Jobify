namespace Jobify.Application.UseCases.AuditLogs.Dtos;

public record GetAuditLogByJobListingResponse
{
    public Guid Id { get; init; }

    public required string EntityType { get; init; }

    public required string Action { get; init; }

    public Guid ChangedBy { get; init; }
    public string? ChangedByType { get; init; }

    public DateTime ChangedAt { get; init; }
    public List<AuditLogDetail> AuditLogDetails { get; init; }
    public Guid EntityId { get; init; }
}
