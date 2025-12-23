namespace Jobify.Application.UseCases.JobListings.Commands.UpdateJobListing;

public class UpdateJobListingCommandHandler : BaseSetting, IRequestHandler<UpdateJobListingCommand, Unit>
{
    private readonly ILogger<UpdateJobListingCommandHandler> _logger;
    private readonly IDistributedCache _cache;

    public UpdateJobListingCommandHandler(
        IApplicationDbContext dbContext,
        ILogger<UpdateJobListingCommandHandler> logger,
        IDistributedCache cache) : base(dbContext)
    {
        _logger = logger;
        _cache = cache;
    }

    public async Task<Unit> Handle(UpdateJobListingCommand request, CancellationToken cancellationToken)
    {
        var jobListing = await _dbContext.JobListings
                          .Where(c => c.Id == request.Id)
                          .FirstOrDefaultAsync(cancellationToken)
                      ?? throw new NotFoundException("JobListing not found");

        jobListing.MapFrom(request);

        await _dbContext.SaveChangesAsync(cancellationToken);

        var cacheKey = $"jobListing:{request.Id}";;
        _logger.LogInformation("invalidating cache for key: {CacheKey} from cache.", cacheKey);
        await _cache.RemoveAsync(cacheKey, cancellationToken);

        return Unit.Value;
    }
}
