using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Jobify.IntegrationTests;

public sealed class CustomWebApplicationFactory
    : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureHostConfiguration(c =>
        {
            var settings = new Dictionary<string, string?>
            {
                // Connection strings
                ["ConnectionStrings:Postgres"] = IntegrationTestEnvironment.PostgresConnectionString,
                ["ConnectionStrings:Redis"] = IntegrationTestEnvironment.RedisConnectionString,
                ["ConnectionStrings:RabbitMq"] = IntegrationTestEnvironment.RabbitMqConnectionString,

                // JWT settings
                ["JwtSettings:SecretKey"] = "YourDevelopmentSecretKeyHere",
                ["JwtSettings:Issuer"] = "JobifyDevIssuer",
                ["JwtSettings:Audience"] = "JobifyDevAudience",
                ["JwtSettings:ExpirationInMinutes"] = "60",
                ["JwtSettings:RefreshTokenExpirationInDays"] = "7"
            };
            c.AddInMemoryCollection(settings);
        });

        return base.CreateHost(builder);
    }
}
