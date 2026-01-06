namespace Jobify.Application.UseCases.JobListings.Commands.DeleteJobListing;

public class DeleteJobListingCommandHandler : BaseSetting, IRequestHandler<DeleteJobListingCommand, Unit>
{
    private readonly IAuthenticatedUser _authenticatedUser;
    private readonly ILogger<DeleteJobListingCommandHandler> _logger;
    private readonly IDistributedCache _cache;

    public DeleteJobListingCommandHandler( IApplicationDbContext dbContext,
        IAuthenticatedUser authenticatedUser, IDistributedCache cache,
        ILogger<DeleteJobListingCommandHandler> logger) : base(dbContext)
    {
        _authenticatedUser = authenticatedUser;
        _cache = cache;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteJobListingCommand request, CancellationToken cancellationToken)
    {
        var jobListing = await _dbContext.JobListings
            .FirstOrDefaultAsync(x => x.Id == request.Id
                                      && x.CreatedBy == _authenticatedUser.Id, cancellationToken)
                         ?? throw new NotFoundException("JobListing not found");

        jobListing.IsDeleted = true;

        await _dbContext.SaveChangesAsync(cancellationToken);

        var cacheKey = $"jobListing:{request.Id}";;
        _logger.LogInformation("invalidating cache for key: {CacheKey} from cache.", cacheKey);
        await _cache.RemoveAsync(cacheKey, cancellationToken);

        return Unit.Value;
    }
}
