namespace Jobify.Application.UseCases.UserProfiles.Command.CreateUserProfiles;

public record CreateUserProfileCommand(
    string FirstName,
    string LastName,
    string PhoneNumber,
    string? Location,
    string? Bio,
    string? Education,
    string? Experience) : IRequest<Unit>;
