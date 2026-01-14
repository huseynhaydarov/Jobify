namespace Jobify.Application.UseCases.JobListings.Commands.UpdateJobListing;

public class UpdateJobListingCommandHandler : BaseSetting,
    IRequestHandler<UpdateJobListingCommand, UpdateJobListingResponse>
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<UpdateJobListingCommandHandler> _logger;
    private readonly IAuthenticatedUser _authenticatedUser;

    public UpdateJobListingCommandHandler(
        IApplicationDbContext dbContext,
        ILogger<UpdateJobListingCommandHandler> logger,
        IDistributedCache cache,
        IAuthenticatedUser authenticatedUser) : base(dbContext)
    {
        _logger = logger;
        _cache = cache;
        _authenticatedUser = authenticatedUser;
    }

    public async Task<UpdateJobListingResponse> Handle(UpdateJobListingCommand request,
        CancellationToken cancellationToken)
    {
        JobListing jobListing = await _dbContext.JobListings
                                    .Where(c => c.Id == request.Id &&
                                                c.CreatedBy == _authenticatedUser.Id)
                                    .FirstOrDefaultAsync(cancellationToken)
                                ?? throw new NotFoundException("JobListing not found");

        jobListing.MapFrom(request);

        await _dbContext.SaveChangesAsync(cancellationToken);

        string cacheKey = $"jobListing:{request.Id}";
        ;
        _logger.LogInformation("invalidating cache for key: {CacheKey} from cache.", cacheKey);
        await _cache.RemoveAsync(cacheKey, cancellationToken);

        return new UpdateJobListingResponse(
            jobListing.Id,
            jobListing.Status,
            jobListing.ModifiedAt);
    }
}
