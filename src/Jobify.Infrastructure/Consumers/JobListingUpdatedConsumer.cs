using Jobify.Application.Common.Models.Caching;
using Jobify.Contracts.JobListings.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Jobify.Infrastructure.Consumers;

public class JobListingUpdatedConsumer : IConsumer<JobListingUpdatedEvent>
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

    public async Task Consume(ConsumeContext<JobListingUpdatedEvent> context)
    {
        _logger.LogInformation("Consuming jobListing event data: {jobListingChangedEvent}", context.Message);

        var db = _redis.GetDatabase();

        string cacheKey = $"jobListing:{context.Message.Id}";
        _logger.LogInformation("invalidating cache for key: {CacheKey} from cache.", cacheKey);
        db.KeyDelete(cacheKey);

        await db.KeyDeleteAsync(JobListingsCacheKeys.Registry);
    }
}
