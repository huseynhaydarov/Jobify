namespace Jobify.Application.Common.Interfaces.Services;

public class IAuthenticatedUser
{
    string? Id { get; }
    List<string>? Roles { get; }
}
