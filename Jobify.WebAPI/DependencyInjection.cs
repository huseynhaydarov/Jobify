using Microsoft.OpenApi.Models;

namespace Jobify.API;

public static class DependencyInjection
{
    public static IServiceCollection AddAPIServices(this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "TajMaster API", Version = "v1" });
        });
        
        return services;
    }
}