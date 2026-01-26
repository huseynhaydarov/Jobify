using Jobify.Infrastructure.Grpc;
using Jobify.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Npgsql;
using StackExchange.Redis;

namespace Jobify.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Postgres");

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

        dataSourceBuilder.EnableDynamicJson();

        var dataSource = dataSourceBuilder.Build();

        services.AddDbContext<ApplicationDbContext>((db, options) =>
        {
            options.UseNpgsql(dataSource);
            options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
        });

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
            options.ConfigurationOptions = new ConfigurationOptions
            {
                AbortOnConnectFail = true, EndPoints = { options.Configuration }
            };
        });

        services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")));

        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        services.AddSingleton<IPasswordHasherService, PasswordHasherService>();
        services.AddScoped<ITokenService, TokenService>();

        services.AddSearchGrpcClient();

        return services;
    }
}
