using Jobify.Application.Common.Models.Caching;
using Jobify.Contracts.JobListings.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Jobify.Infrastructure.Consumers;

public class JobListingAddedConsumer : IConsumer<JobListingCreatedEvent>
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

    public async Task Consume(ConsumeContext<JobListingCreatedEvent> context)
    {
        _logger.LogInformation("Consuming jobListing event data: {jobListingEvent}", context.Message);

        var db = _redis.GetDatabase();

        await db.KeyDeleteAsync(JobListingsCacheKeys.Registry);
    }
}
