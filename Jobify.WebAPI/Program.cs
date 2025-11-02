using Jobify.API;
using Jobify.API.Middlewares;
using Jobify.Application;
using Jobify.Infrastructure;
using Jobify.Infrastructure.Persistence.Data;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services, builder.Configuration);

builder.Services.AddOpenApi();

var app = builder.Build();

await app.InitialiseDatabaseAsync();

ConfigureMiddleware(app);

app.Run();

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        });
    });

    services.AddControllers();

    services
        .AddInfrastructureServices(configuration)
        .AddApplicationServices(configuration)
        .AddAPIServices(configuration);
}


void ConfigureMiddleware(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Jobify API v1");
        });
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
    }

    app.UseHttpsRedirection();
    app.UseCors("AllowAll");

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseMiddleware<CustomExceptionHandlerMiddleware>();

    app.MapControllers();
}
