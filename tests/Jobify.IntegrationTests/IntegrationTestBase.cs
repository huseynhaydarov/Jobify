using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using Jobify.Application.Common.Interfaces.Services;
using Jobify.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Jobify.IntegrationTests;

public abstract class IntegrationTestBase
{
    protected HttpClient Client { get; private set; }
    protected CustomWebApplicationFactory Factory { get; private set; }
    protected Fixture? Fixture { get; private set; }

    [SetUp]
    public void SetUp()
    {
        Factory = new CustomWebApplicationFactory();
        Client = Factory.CreateClient();
        Fixture = new Fixture();
    }

    [TearDown]
    public async Task TearDown()
    {
        Client?.Dispose();
        Factory?.Dispose();

        await DatabaseReset.ResetAsync(
            IntegrationTestEnvironment.PostgresConnectionString);
    }

    protected async Task<string> GetJwtTokenByRoleAsync(string role)
    {
        using var scope = Factory.Services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();

        var user = await db.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstAsync(u => u.UserRoles.Any(ur => ur.Role!.Name == role));

        return tokenService.GenerateJwtToken(user);
    }
}
