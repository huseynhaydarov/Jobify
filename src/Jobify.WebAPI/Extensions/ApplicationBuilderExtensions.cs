using Jobify.Infrastructure.Persistence.Data;

namespace Jobify.WebAPI.Extensions;

public static class ApplicationBuilderExtensions
{
    public static async Task InitDbAsync(
        this WebApplication app) =>
        await DatabaseInitializer.InitializeAsync(app.Services);
}
