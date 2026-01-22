using Jobify.Contracts.JobListings.Events;
using MassTransit;

namespace Jobify.Infrastructure.Consumers;

public class JobListingAddedAuditLogConsumer : IConsumer<JobListingCreatedEvent>
{
    public Task Consume(ConsumeContext<JobListingCreatedEvent> context) => Task.CompletedTask;
}
