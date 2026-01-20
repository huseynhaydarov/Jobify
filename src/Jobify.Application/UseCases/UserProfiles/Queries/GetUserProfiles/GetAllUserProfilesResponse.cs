using System;

namespace Jobify.Application.UseCases.UserProfiles.Queries.GetUserProfiles;

public class GetAllUserProfilesResponse
{
    public Guid Id { get; init; }
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string? Location { get; set; }
    public string? Bio { get; set; }
    public string? Education { get; set; }
    public string? Experience { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
