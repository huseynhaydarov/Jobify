using Jobify.Application.Common.Models.Caching;
using Jobify.Application.UseCases.JobListings.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Jobify.Infrastructure.Consumers;

public class JobListingUpdatedConsumer : IConsumer<JobListingChangedEvent>
{
    private readonly IConnectionMultiplexer _redis;
    private readonly ILogger<JobListingUpdatedConsumer> _logger;

    public JobListingUpdatedConsumer(
        IConnectionMultiplexer redis,
        ILogger<JobListingUpdatedConsumer> logger)
    {
        _redis = redis;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<JobListingChangedEvent> context)
    {
        _logger.LogInformation("Consuming jobListing event data: {jobListingChangedEvent}", context.Message);

        var db = _redis.GetDatabase();

        await db.KeyDeleteAsync(JobListingsCacheKeys.Registry);
    }
}
