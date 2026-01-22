namespace Jobify.Domain.Entities;

public class AuditLog
{
    public Guid Id { get; set; }

    public required string EntityType { get; set; }

    public AuditAction Action { get; set; }

    public Guid ChangedBy { get; set; }
    public string? ChangedByType { get; set; }

    public DateTime ChangedAt { get; set; }

    public string? Changes { get; set; }
    public Guid EntityId { get; set; }
}
