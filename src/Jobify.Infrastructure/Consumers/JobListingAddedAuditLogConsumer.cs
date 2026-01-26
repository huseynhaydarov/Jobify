using Jobify.Contracts.JobListings.Events;
using Jobify.Domain.Enums;
using Jobify.Infrastructure.Persistence;
using MassTransit;

namespace Jobify.Infrastructure.Consumers;

public class JobListingAddedAuditLogConsumer : IConsumer<JobListingCreatedEvent>
{
    private readonly ApplicationDbContext _dbContext;

    public JobListingAddedAuditLogConsumer(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<JobListingCreatedEvent> context)
    {
        var message = context.Message;

        var auditLog = new AuditLog
        {
            Id = Guid.NewGuid(),
            EntityType = nameof(JobListing),
            EntityId = message.Id,
            Action = AuditAction.Created,
            ChangedAt = DateTime.UtcNow,
            ChangedBy = message.CreatedById ?? Guid.Empty,
            ChangedByType = message.CreatedBy,
            AuditLogDetails =
            [
                new AuditLogDetail
                {
                    PropertyName = nameof(message.Name),
                    NewValue = message.Name
                },
                new AuditLogDetail
                {
                    PropertyName = nameof(message.Description),
                    NewValue = message.Description
                },
                new AuditLogDetail
                {
                    PropertyName = nameof(message.Requirements),
                    NewValue = message.Requirements
                },
                new AuditLogDetail
                {
                    PropertyName = nameof(message.Location),
                    NewValue = message.Location
                },
                new AuditLogDetail
                {
                    PropertyName = nameof(message.Salary),
                    NewValue = message.Salary.ToString()
                },
                new AuditLogDetail
                {
                    PropertyName = nameof(message.Currency),
                    NewValue = message.Currency
                },
                new AuditLogDetail
                {
                    PropertyName = nameof(message.Status),
                    NewValue = message.Status
                },
                new AuditLogDetail
                {
                    PropertyName = nameof(message.ExpiresAt),
                    NewValue = message.ExpiresAt?.ToString()
                }
            ]
        };

        _dbContext.AuditLogs.Add(auditLog);
        await _dbContext.SaveChangesAsync(context.CancellationToken);
    }
}
