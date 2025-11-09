namespace Jobify.Domain.Entities;

public class Role : BaseAuditableEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required bool IsActive { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}

