namespace Jobify.IntegrationTests.Infrastructure;

public sealed class CustomWebApplicationFactory<TEntryPoint>
    : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
{
    private readonly PostgreSqlContainer _postgres =
        new PostgreSqlBuilder("postgres:17-alpine")
            .WithDatabase("jobifyTestDb")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithCleanUp(true)
            .Build();

    private readonly RedisContainer _redis =
        new RedisBuilder("redis:7-alpine")
            .WithCleanUp(true)
            .Build();

    public async Task InitializeAsync()
    {
        await Task.WhenAll(
            _postgres.StartAsync(),
            _redis.StartAsync()
        );

        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasherService>();
        /*await dbContext.Database.MigrateAsync();*/
        await DatabaseSeeder.SeedTestUsersAsync(dbContext,  hasher);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        //builder.UseEnvironment("Test");

        builder.UseSetting(
            "ConnectionStrings:DefaultConnection",
            _postgres.GetConnectionString());

        builder.UseSetting(
            "ConnectionStrings:Redis",
            _redis.GetConnectionString());

        builder.ConfigureTestServices(services =>
        {
            services.AddSingleton<IAuhtenticatedTestUser, AuthenticatedTestUserProvider>();

            services.AddSingleton<IPasswordHasherService, PasswordHasherService>();

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "TestScheme";
                    options.DefaultChallengeScheme = "TestScheme";
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    "TestScheme", _ => { });
        });
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();

        await Task.WhenAll(
            _postgres.DisposeAsync().AsTask(),
            _redis.DisposeAsync().AsTask()
        );
    }
}
