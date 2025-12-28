namespace Application.UnitTests.Common;

public sealed class TestAuthenticatedUser : IAuthenticatedUser
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public List<string>? Roles { get; }
}
