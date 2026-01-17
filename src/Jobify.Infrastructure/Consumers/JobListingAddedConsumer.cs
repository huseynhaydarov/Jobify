using Jobify.Application.Common.Models.Caching;
using Jobify.Application.UseCases.JobListings.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Jobify.Infrastructure.Consumers;

public class JobListingAddedConsumer : IConsumer<JobListingChangedEvent>
{
    private readonly IConnectionMultiplexer _redis;
    private readonly ILogger<JobListingAddedConsumer> _logger;

    public JobListingAddedConsumer(
        IConnectionMultiplexer redis,
        ILogger<JobListingAddedConsumer> logger)
    {
        _redis = redis;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<JobListingChangedEvent> context)
    {
        _logger.LogInformation("Consuming jobListing event data: {jobListingEvent}", context.Message);

        var db = _redis.GetDatabase();

        var fields = await db.HashKeysAsync(JobListingsCacheKeys.Registry);

        if (fields.Length == 0)
        {
            _logger.LogInformation("No cached pages to invalidate.");
            return;
        }

        var deleteTasks = fields
            .Select(key => db.KeyDeleteAsync(key.ToString()));

        await Task.WhenAll(deleteTasks);

        await db.KeyDeleteAsync(JobListingsCacheKeys.Registry);
    }
}
