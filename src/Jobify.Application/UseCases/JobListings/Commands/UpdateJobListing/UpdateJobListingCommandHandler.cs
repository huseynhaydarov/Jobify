using Jobify.Contracts.JobListings.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Jobify.Application.UseCases.JobListings.Commands.UpdateJobListing;

public class UpdateJobListingCommandHandler : BaseSetting,
    IRequestHandler<UpdateJobListingCommand, UpdateJobListingResponse>
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<UpdateJobListingCommandHandler> _logger;
    private readonly IAuthenticatedUserService _authenticatedUserService;
    private readonly IPublishEndpoint _publishEndpoint;

    public UpdateJobListingCommandHandler(
        IApplicationDbContext dbContext,
        ILogger<UpdateJobListingCommandHandler> logger,
        IDistributedCache cache,
        IAuthenticatedUserService authenticatedUserService, IPublishEndpoint publishEndpoint) : base(dbContext)
    {
        _logger = logger;
        _cache = cache;
        _authenticatedUserService = authenticatedUserService;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<UpdateJobListingResponse> Handle(UpdateJobListingCommand request,
        CancellationToken cancellationToken)
    {
        JobListing jobListing = await _dbContext.JobListings
                                    .Where(c => c.Id == request.Id &&
                                                c.CreatedBy == _authenticatedUserService.Id)
                                    .FirstOrDefaultAsync(cancellationToken)
                                ?? throw new NotFoundException("JobListing not found");

        jobListing.MapFrom(request);

        var entry = _dbContext.Entry(jobListing);
        var changes = GetChanges(entry);

        await _dbContext.SaveChangesAsync(cancellationToken);

        await _publishEndpoint.Publish(new JobListingUpdatedEvent
        {
            Id = jobListing.Id,
            Name = jobListing.Name,
            Description = jobListing.Description,
            Requirements = jobListing.Requirements,
            Location = jobListing.Location,
            Salary =  jobListing.Salary,
            Status = jobListing.Status.ToString(),
            Currency = jobListing.Currency,
            ExpireDate = jobListing.ExpiresAt,
            Changes = changes,
            ModifiedAt = jobListing.ModifiedAt ??  DateTime.UtcNow,
            ModifiedBy = string.Join(", ", _authenticatedUserService.Roles!),
            ModifiedById = jobListing.ModifiedBy ?? Guid.Empty,
        }, cancellationToken);

        return new UpdateJobListingResponse(
            jobListing.Id,
            jobListing.Status,
            jobListing.ModifiedAt);
    }

    private static List<AuditLogDetail> GetChanges(EntityEntry entry)
    {
        var changes = new List<AuditLogDetail>();

        foreach (var property in entry.Properties)
        {
            if (!property.IsModified)
                continue;

            changes.Add(new AuditLogDetail
            {
                PropertyName = property.Metadata.Name,
                OldValue = property.OriginalValue?.ToString(),
                NewValue = property.CurrentValue?.ToString()
            });
        }
        return changes;
    }

}
