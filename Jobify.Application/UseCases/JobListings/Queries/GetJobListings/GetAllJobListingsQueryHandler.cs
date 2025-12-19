namespace Jobify.Application.UseCases.JobListings.Queries.GetJobListings;

public class GetAllJobListingsQueryHandler : BaseSetting,
    IRequestHandler<GetAllJobListingsQuery, PaginatedResult<GetAllJobListingsResponse>>
{
    private readonly ILogger<GetAllJobListingsQueryHandler> _logger;
    private readonly IDistributedCache _cache;

    public GetAllJobListingsQueryHandler(
        IApplicationDbContext dbContext,
        ILogger<GetAllJobListingsQueryHandler> logger,
        IDistributedCache cache) : base(dbContext)
    {
        _logger = logger;
        _cache = cache;
    }

    public async Task<PaginatedResult<GetAllJobListingsResponse>> Handle(GetAllJobListingsQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = "joblistings";
        _logger.LogInformation("fetching data for key: {CacheKey} from cache" , cacheKey);

        var cacheOptions = new DistributedCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(2))
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(20));

        var jobListings = await _cache.GetOrSetAsync(
            cacheKey,
            async () =>
            {
                _logger.LogInformation("cache miss. fetching data for key: {CacheKey} from database", cacheKey);
                var queryable = _dbContext.JobListings
                    .AsNoTracking()
                    .Select(c => new GetAllJobListingsResponse
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        Requirements = c.Requirements,
                        Location = c.Location,
                        Salary = c.Salary,
                        Currency = c.Currency,
                        Status = c.Status,
                        PostedAt = c.PostedAt,
                        Views = c.Views,
                    });

                return await queryable.PaginatedListAsync(
                    request.Parameters.PageNumber,
                    request.Parameters.PageSize,
                    cancellationToken);
            },
            cacheOptions);

        if (jobListings == null)
        {
            throw new NullDataException("Retrieved data is null.");
        }

        return jobListings;
    }
}
