using Jobify.Application.UseCases.JobListings.Events;
using Jobify.Contracts.JobListings.IntegrationEvents;
using MassTransit;

namespace Jobify.Application.UseCases.JobListings.Commands.DeleteJobListing;

public class DeleteJobListingCommandHandler : BaseSetting,
    IRequestHandler<DeleteJobListingCommand, CloseJobListingResponse>
{
    private readonly IAuthenticatedUserService _authenticatedUserService;
    private readonly IDistributedCache _cache;
    private readonly ILogger<DeleteJobListingCommandHandler> _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    public DeleteJobListingCommandHandler(IApplicationDbContext dbContext,
        IAuthenticatedUserService authenticatedUserService, IDistributedCache cache,
        ILogger<DeleteJobListingCommandHandler> logger, IPublishEndpoint publishEndpoint) : base(dbContext)
    {
        _authenticatedUserService = authenticatedUserService;
        _cache = cache;
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<CloseJobListingResponse> Handle(DeleteJobListingCommand request,
        CancellationToken cancellationToken)
    {
        var jobListing = await _dbContext.JobListings
                             .FirstOrDefaultAsync(x => x.Id == request.Id
                                                       && x.CreatedBy == _authenticatedUserService.Id,
                                 cancellationToken)
                         ?? throw new NotFoundException("JobListing not found");

        jobListing.IsDeleted = true;
        jobListing.Status = JobStatus.Closed;
        jobListing.ClosedAt = DateTimeOffset.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        var jobListingDeletedEvent = new JobListingChangedEvent { Id = jobListing.Id, Action = ActionType.Deleted };

        await _publishEndpoint.Publish(jobListingDeletedEvent, cancellationToken);

        await _publishEndpoint.Publish(new JobListingDeleted { Id = jobListing.Id }, cancellationToken);

        var cacheKey = $"jobListing:{request.Id}";
        _logger.LogInformation("invalidating cache for key: {CacheKey} from cache.", cacheKey);
        await _cache.RemoveAsync(cacheKey, cancellationToken);

        return new CloseJobListingResponse(
            jobListing.Id,
            jobListing.Status,
            jobListing.ClosedAt);
    }
}
