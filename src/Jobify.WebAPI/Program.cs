using Jobify.Application.UseCases.JobListings.Dtos;
using Jobify.WebAPI;
using Jobify.WebAPI.Middlewares;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

ODataConventionModelBuilder modelBuilder = new();
modelBuilder.EntitySet<JobListingOdataDto>("JobListings");

ConfigureServices(builder.Services, builder.Configuration);

WebApplication app = builder.Build();

ConfigureMiddleware(app);

await ApplyMigrationsAsync(app);

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
    });


    services.AddOpenApi();

    services.AddControllers().AddOData(options => options
        .Select()
        .Filter()
        .OrderBy()
        .Expand()
        .Count()
        .SetMaxTop(null)
        .AddRouteComponents(
            "odata",
            modelBuilder.GetEdmModel()));

    services.AddHttpContextAccessor();

    services
        .AddInfrastructureServices(configuration)
        .AddApplicationServices(configuration)
        .AddWebServices(configuration);
}

void ConfigureMiddleware(WebApplication app)
{
    app.UseMiddleware<CustomExceptionHandlerMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHttpsRedirection();
    }

    app.UseCors("AllowAll");

    app.UseRouting();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();
}

async Task ApplyMigrationsAsync(WebApplication app)
{
    try
    {
        using IServiceScope scope = app.Services.CreateScope();
        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        ILogger<Program> logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while applying database migrations!");
        throw;
    }
}

app.Run();
