namespace Jobify.IntegrationTests.Common;

public class AuthenticatedTestUserProvider : IAuhtenticatedTestUser
{
    public Guid Id { get; set; }
    public List<string> Roles { get; set; } = new();
}
