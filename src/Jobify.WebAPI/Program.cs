using Jobify.Application.UseCases.JobListings.Dtos;
using Jobify.WebAPI;
using Jobify.WebAPI.Extensions;
using Jobify.WebAPI.Middlewares;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

ODataConventionModelBuilder modelBuilder = new();
modelBuilder.EntitySet<JobListingOdataDto>("JobListings");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddOpenApi();

builder.Services.AddControllers()
    .AddOData(options => options
        .Select()
        .Filter()
        .OrderBy()
        .Expand()
        .Count()
        .SetMaxTop(null)
        .AddRouteComponents("odata", modelBuilder.GetEdmModel()));

builder.Services.AddHttpContextAccessor();

builder.Services
    .AddInfrastructureServices(builder.Configuration)
    .AddApplicationServices(builder.Configuration)
    .AddWebServices(builder.Configuration);

WebApplication app = builder.Build();

await app.InitDbAsync();

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

app.Run();

public partial class Program
{
}
