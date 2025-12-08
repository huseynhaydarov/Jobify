namespace Jobify.Domain.Entities;

public class UserProfile : BaseAuditableEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; }

    public required string  FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PhoneNumber { get; set; }
    public string? Location { get; set; }
    public string? Bio { get; set; }
    public string? Education { get; set; }
    public string? Experience { get; set; }
    public bool IsDeleted { get; set; }
}
