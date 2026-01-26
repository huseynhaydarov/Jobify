using System.Threading.Tasks;
using Jobify.Application.Common.Models.Caching;
using Jobify.Contracts.JobListings.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Jobify.Infrastructure.Consumers;

public class JobListingDeletedConsumer : IConsumer<JobListingDeletedEvent>
{
    private readonly ILogger<JobListingDeletedConsumer> _logger;
    private readonly IConnectionMultiplexer _redis;

    public JobListingDeletedConsumer(
        IConnectionMultiplexer redis,
        ILogger<JobListingDeletedConsumer> logger)
    {
        _redis = redis;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<JobListingDeletedEvent> context)
    {
        _logger.LogInformation("Consuming jobListing event data: {jobListingEvent}", context.Message);

        var db = _redis.GetDatabase();

        var cacheKey = $"jobListing:{context.Message.Id}";
        _logger.LogInformation("invalidating cache for key: {CacheKey} from cache.", cacheKey);

        await db.KeyDeleteAsync([cacheKey, JobListingsCacheKeys.Registry]);
    }
}
