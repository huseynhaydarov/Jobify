var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

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

    services.AddControllers();

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
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while applying database migrations!");
        throw;
    }
}

app.Run();
