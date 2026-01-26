using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Testing;
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
            Dictionary<string, string?> settings = new()
            {
                // Connection strings
                ["ConnectionStrings:Postgres"] = IntegrationTestEnvironment.PostgresConnectionString,
                ["ConnectionStrings:Redis"] = IntegrationTestEnvironment.RedisConnectionString,
                ["ConnectionStrings:RabbitMq"] = IntegrationTestEnvironment.RabbitMqConnectionString,

                // JWT
                ["JwtSettings:SecretKey"] = JwtTokenTestSettings.SecretKey,
                ["JwtSettings:Issuer"] = JwtTokenTestSettings.Issuer,
                ["JwtSettings:Audience"] = JwtTokenTestSettings.Audience,
                ["JwtSettings:ExpirationMinutes"] = JwtTokenTestSettings.ExpirationMinutes.ToString(),
                ["JwtSettings:RefreshTokenExpirationDays"] = JwtTokenTestSettings.RefreshTokenExpirationDays.ToString(),

                // MassTransit
                ["MassTransit:Url"] = "localhost",
                ["MassTransit:Host"] = "/",
                ["MassTransit:Username"] = "guest",
                ["MassTransit:Password"] = "guest"
            };

            c.AddInMemoryCollection(settings);
        });

        return base.CreateHost(builder);
    }
}
