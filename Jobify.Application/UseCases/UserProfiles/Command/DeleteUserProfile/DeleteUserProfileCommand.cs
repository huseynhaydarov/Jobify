namespace Jobify.Application.UseCases.UserProfiles.Command.DeleteUserProfile;

public record DeleteUserProfileCommand(Guid Id) : IRequest<Unit>;
