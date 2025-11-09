namespace Jobify.Application.Common.Interfaces.Services;

public interface IAuthenticatedUser
{
    Guid? Id { get; }
    List<string>? Roles { get; }
}
