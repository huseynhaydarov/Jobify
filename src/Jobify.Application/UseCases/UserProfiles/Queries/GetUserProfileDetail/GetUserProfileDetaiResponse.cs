using System;

namespace Jobify.Application.UseCases.UserProfiles.Queries.GetUserProfileDetail;

public record GetUserProfileDetailResponse
{
    public Guid Id { get; init; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }

    public string? Location { get; set; }
    public string? Bio { get; set; }
    public string? Education { get; set; }
    public string? Experience { get; set; }
}
