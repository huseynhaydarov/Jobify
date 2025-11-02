using Jobify.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Jobify.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public required bool IsActive { get; set; }

    public ICollection<IdentityUserRole<Guid>> UserRoles { get; set; } = new List<IdentityUserRole<Guid>>();
    public ICollection<JobListing> JobListings { get; set; } =  new List<JobListing>();
    public ICollection<JobApplication>  Applications { get; set; } = new List<JobApplication>();
    public ICollection<Message> SentMessages { get; set; } = new List<Message>();
    public ICollection<Message> ReceivedMessages  { get; set; } = new List<Message>();

    public ICollection<Company> Companies { get; set; } = new List<Company>();
}

