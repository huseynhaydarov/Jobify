namespace Jobify.Application.UseCases.UserProfiles.Queries.GetUserProfiles;

public class GetAllUserProfilesQueryHandler : BaseSetting, IRequestHandler<GetAllUserProfilesQuery,
    PaginatedResult<GetAllUserProfilesResponse>>
{
    private readonly ILogger<GetAllUserProfilesQueryHandler> _logger;
    private readonly IDistributedCache _cache;

    public GetAllUserProfilesQueryHandler(
        IApplicationDbContext dbContext,
        ILogger<GetAllUserProfilesQueryHandler> logger,
        IDistributedCache cache) : base(dbContext)
    {
        _logger = logger;
        _cache = cache;
    }

    public async Task<PaginatedResult<GetAllUserProfilesResponse>> Handle(GetAllUserProfilesQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = "userProfiles";
        _logger.LogInformation("fetching data for key: {CacheKey} from cache", cacheKey);

        var cacheOptions = new DistributedCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(2))
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(20));

        var userProfiles = await _cache.GetOrSetAsync(
            cacheKey,
            async () =>
            {
                _logger.LogInformation("cache miss. fetching data for key: {CacheKey} from database", cacheKey);
                var queryable = _dbContext.UserProfiles
                    .OrderByDescending(x => x.CreatedAt)
                    .Select(p => new GetAllUserProfilesResponse()
                    {
                        Id = p.Id,
                        FullName = p.FirstName + " " + p.LastName,
                        PhoneNumber = p.PhoneNumber,
                        Location = p.Location,
                        Bio = p.Bio,
                        Education = p.Education,
                        Experience = p.Experience,
                        CreatedAt = p.CreatedAt,
                        UpdatedAt = p.ModifiedAt
                    });

                return await queryable.PaginatedListAsync(
                    request.PagingParameters.PageNumber,
                    request.PagingParameters.PageSize,
                    cancellationToken);
            },
            cacheOptions);

        if (userProfiles == null)
        {
            throw new NullDataException("Retrieved data is null.");
        }

        return userProfiles;
    }
}
