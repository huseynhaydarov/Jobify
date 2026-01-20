using Jobify.Infrastructure.Persistence.Data.Seed;
using Microsoft.Extensions.Logging;

namespace Jobify.Infrastructure.Persistence.Data;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(
        IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var logger = scope.ServiceProvider
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger("DatabaseInitializer");

        try
        {
            var dbContext = scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            logger.LogInformation("Applying database migrations...");
            await dbContext.Database.MigrateAsync();

            logger.LogInformation("Seeding database...");
            await DatabaseSeeder.SeedAsync(scope.ServiceProvider);
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Database initialization failed");
            throw;
        }
    }
}
