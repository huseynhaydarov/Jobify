namespace Jobify.Application.UseCases.UserProfiles.Command.UpdateUserProfile;

public record UpdateUserProfileCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string? Location,
    string? Bio,
    string? Education,
    string? Experience) : IRequest<Unit>;
