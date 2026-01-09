namespace Jobify.Application.UseCases.UserProfiles.Command.CreateUserProfile;

public record CreateUserProfileCommand(
    string FirstName,
    string LastName,
    string PhoneNumber,
    string? Location,
    string? Bio,
    string? Education,
    string? Experience) : IRequest<UserProfileDto>;
