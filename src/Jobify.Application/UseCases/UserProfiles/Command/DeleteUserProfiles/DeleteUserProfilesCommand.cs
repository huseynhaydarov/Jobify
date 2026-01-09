namespace Jobify.Application.UseCases.UserProfiles.Command.DeleteUserProfiles;

public record DeleteUserProfilesCommand(List<Guid> Ids) : IRequest<Unit>;
