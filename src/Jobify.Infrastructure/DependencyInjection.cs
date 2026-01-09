using StackExchange.Redis;

namespace Jobify.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>((db, options) => { options.UseNpgsql(connectionString); });

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
            options.ConfigurationOptions = new ConfigurationOptions
            {
                AbortOnConnectFail = true, EndPoints = { options.Configuration }
            };
        });

        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        services.AddSingleton<IPasswordHasherService, PasswordHasherService>();
        services.AddScoped<ITokenService, TokenService>();


        return services;
    }
}
