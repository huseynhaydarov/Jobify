namespace Jobify.Contracts.JobListings.Models;

public class AuditLogDetail
{
    public string PropertyName { get; set; }
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
}
