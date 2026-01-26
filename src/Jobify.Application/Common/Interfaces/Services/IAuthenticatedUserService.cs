namespace Jobify.Application.Common.Interfaces.Services;

public interface IAuthenticatedUserService
{
    Guid? Id { get; }
    List<string>? Roles { get; }
    string Email { get; }
}
