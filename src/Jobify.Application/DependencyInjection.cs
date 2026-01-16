using Jobify.Application.UseCases.JobApplications.Commands.CreateJobApplication;

namespace Jobify.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            config.AddOpenBehavior(typeof(UnhandledExceptionBehaviour<,>));
            config.AddOpenRequestPreProcessor(typeof(LoggingBehaviour<>));
            config.AddOpenBehavior(typeof(PerformanceBehaviour<,>));
        });

        services.AddValidatorsFromAssembly(typeof(CreateJobApplicationCommandValidator).Assembly);

        return services;
    }
}
