namespace Jobify.Domain.Entities;

public class Message : BaseAuditableEntity
{
    public required Guid SenderId { get; set; }
    public User? Sender { get; set; }
    public required Guid ReceiverId { get; set; }
    public User? Receiver { get; set; }

    public required Guid JobId { get; set; }
    public JobListing? JobListing { get; set; }
    public required string MessageText { get; set; }
}
