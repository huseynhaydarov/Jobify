using Microsoft.AspNetCore.Identity;

namespace Jobify.Infrastructure.Identity;

public class ApplicationRole : IdentityRole<Guid>
{
    public bool? IsActive { get; set; }
    public ICollection<IdentityUserRole<Guid>> UserRoles { get; set; } = new List<IdentityUserRole<Guid>>();
}
