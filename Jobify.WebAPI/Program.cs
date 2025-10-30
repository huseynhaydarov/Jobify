using Jobify.Application;
using Jobify.Infrastructure;
using Jobify.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services, builder.Configuration);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();


ApplyMigrations(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services
        .AddInfrastructureServices(configuration)
        .AddApplicationServices(configuration);
    ;}


void ApplyMigrations(WebApplication app)
{
    try
    {
        var applicationDbContext =
            app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
        applicationDbContext.Database.MigrateAsync().Wait();
    }
    catch (Exception ex)
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while applying database migrations!");
        throw;
    }
}


app.Run();

