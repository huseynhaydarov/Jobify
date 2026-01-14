namespace Jobify.Application.UseCases.JobListings.Commands.DeleteJobListing;

public class DeleteJobListingCommandHandler : BaseSetting,
    IRequestHandler<DeleteJobListingCommand, CloseJobListingResponse>
{
    private readonly IAuthenticatedUser _authenticatedUser;
    private readonly IDistributedCache _cache;
    private readonly ILogger<DeleteJobListingCommandHandler> _logger;

    public DeleteJobListingCommandHandler(IApplicationDbContext dbContext,
        IAuthenticatedUser authenticatedUser, IDistributedCache cache,
        ILogger<DeleteJobListingCommandHandler> logger) : base(dbContext)
    {
        _authenticatedUser = authenticatedUser;
        _cache = cache;
        _logger = logger;
    }

    public async Task<CloseJobListingResponse> Handle(DeleteJobListingCommand request,
        CancellationToken cancellationToken)
    {
        JobListing jobListing = await _dbContext.JobListings
                                    .FirstOrDefaultAsync(x => x.Id == request.Id
                                                              && x.CreatedBy == _authenticatedUser.Id,
                                        cancellationToken)
                                ?? throw new NotFoundException("JobListing not found");

        jobListing.IsDeleted = true;
        jobListing.ClosedAt = DateTimeOffset.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        string cacheKey = $"jobListing:{request.Id}";
        ;
        _logger.LogInformation("invalidating cache for key: {CacheKey} from cache.", cacheKey);
        await _cache.RemoveAsync(cacheKey, cancellationToken);

        return new CloseJobListingResponse(
            jobListing.Id,
            JobStatus.Closed,
            jobListing.ClosedAt);
    }
}
