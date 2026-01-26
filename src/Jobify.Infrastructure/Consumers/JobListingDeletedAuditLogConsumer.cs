using System;
using System.Threading.Tasks;
using Jobify.Contracts.JobListings.Events;
using Jobify.Domain.Enums;
using Jobify.Infrastructure.Persistence;
using MassTransit;

namespace Jobify.Infrastructure.Consumers;

public class JobListingDeletedAuditLogConsumer : IConsumer<JobListingDeletedEvent>
{
    private readonly ApplicationDbContext _dbContext;

    public JobListingDeletedAuditLogConsumer(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<JobListingDeletedEvent> context)
    {
        var message = context.Message;

        var auditLog = new AuditLog
        {
            Id = Guid.NewGuid(),
            EntityType = nameof(JobListing),
            EntityId = message.Id,
            Action = AuditAction.Closed,
            ChangedAt = DateTime.UtcNow,
            ChangedBy = message.ClosedById,
            ChangedByType = message.ClosedBy
        };

        _dbContext.AuditLogs.Add(auditLog);
        await _dbContext.SaveChangesAsync(context.CancellationToken);
    }
}
