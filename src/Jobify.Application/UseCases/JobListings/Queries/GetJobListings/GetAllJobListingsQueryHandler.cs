using Jobify.Application.Common.Models.Caching;
using StackExchange.Redis;

namespace Jobify.Application.UseCases.JobListings.Queries.GetJobListings;

public class GetAllJobListingsQueryHandler : BaseSetting,
    IRequestHandler<GetAllJobListingsQuery, PaginatedResult<GetAllJobListingsResponse>>
{
    private readonly ILogger<GetAllJobListingsQueryHandler> _logger;
    private readonly IDistributedCache _cache;
    private readonly IConnectionMultiplexer _redis;

    public GetAllJobListingsQueryHandler(
        IApplicationDbContext dbContext,
        ILogger<GetAllJobListingsQueryHandler> logger, IDistributedCache cache, IConnectionMultiplexer redis)
        : base(dbContext)
    {
        _logger = logger;
        _cache = cache;
        _redis = redis;
    }

    public async Task<PaginatedResult<GetAllJobListingsResponse>> Handle(
        GetAllJobListingsQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = JobListingsCacheKeys.Page(request.Parameters.PageNumber, request.Parameters.PageSize);
        _logger.LogInformation("Fetching data for key: {CacheKey} from cache", cacheKey);

        var jobListings = await _cache.GetOrSetAsync(
            cacheKey,
            async () =>
            {
                var queryable =
                    _dbContext.JobListings
                        .Where(c => !c.IsDeleted)
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
                            PostedAt = c.PostedAt
                        });

                var db = _redis.GetDatabase();
                await db.SetAddAsync(JobListingsCacheKeys.Registry, cacheKey);

                return await queryable.PaginatedListAsync(
                    request.Parameters.PageNumber,
                    request.Parameters.PageSize,
                    cancellationToken);
            }
        );

        if (jobListings == null)
        {
            throw new NullDataException("Retrieved data is null.");
        }

        return jobListings;
    }
}
