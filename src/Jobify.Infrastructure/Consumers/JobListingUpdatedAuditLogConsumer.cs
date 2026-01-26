using System;
using System.Threading.Tasks;
using Jobify.Contracts.JobListings.Events;
using Jobify.Domain.Enums;
using MassTransit;

namespace Jobify.Infrastructure.Consumers;

public class JobListingUpdatedAuditLogConsumer : IConsumer<JobListingUpdatedEvent>
{
    private readonly IApplicationDbContext _dbContext;

    public JobListingUpdatedAuditLogConsumer(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<JobListingUpdatedEvent> context)
    {
        var message = context.Message;

        var auditLog = new AuditLog
        {
            Id = Guid.NewGuid(),
            EntityType = nameof(JobListing),
            EntityId = message.Id,
            Action = AuditAction.Updated,
            ChangedAt = DateTime.UtcNow,
            ChangedBy = message.ModifiedById,
            ChangedByType = message.ModifiedBy,
            AuditLogDetails = message.Changes
        };

        _dbContext.AuditLogs.Add(auditLog);
        await _dbContext.SaveChangesAsync(context.CancellationToken);
    }
}

