namespace Jobify.IntegrationTests.Common;

public interface IAuhtenticatedTestUser
{
    Guid Id { get; set; }
    List<string> Roles { get; set; }
}
