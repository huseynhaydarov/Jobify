using Jobify.Infrastructure.Consumers;
using Jobify.WebAPI.Configurations;
using MassTransit;

namespace Jobify.WebAPI.Extensions;

public static class MassTransitExtension
{
    public static void AddMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        var massTransitConfig = new MassTransitConfiguration();

        configuration.GetSection(MassTransitConfiguration.Key)
            .Bind(massTransitConfig);

        if (massTransitConfig.Url == null || massTransitConfig.Host == null)
        {
            throw new Exception("Section 'MassTransit' are not found");
        }

        services.AddMassTransit(x =>
        {
            x.AddConsumer<JobListingAddedConsumer>();
            x.AddConsumer<JobListingDeletedConsumer>();
            x.AddConsumer<JobListingUpdatedConsumer>();

            x.SetKebabCaseEndpointNameFormatter();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(massTransitConfig.Url , massTransitConfig.Host, h =>
                {
                    h.Username(massTransitConfig.Username);
                    h.Password(massTransitConfig.Password);
                });

                cfg.UseMessageRetry(retry => {
                    retry.Interval(3,
                    TimeSpan.FromSeconds(5));
                });

                cfg.ReceiveEndpoint("add-jobListing-queue",e =>
                {
                    e.ConfigureConsumer<JobListingAddedConsumer>(context);
                });

                cfg.ReceiveEndpoint("delete-jobListing-queue",e =>
                {
                    e.ConfigureConsumer<JobListingDeletedConsumer>(context);
                });

                cfg.ReceiveEndpoint("update-jobListing-queue", e =>
                {
                    e.ConfigureConsumer<JobListingUpdatedConsumer>(context);
                });
            });
        });

    }
}
