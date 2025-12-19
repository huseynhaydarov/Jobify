namespace Jobify.Application.UseCases.UserProfiles.Queries.GetUserProfileDetail;

public class GetUserProfileDetailQueryHandler : BaseSetting,
    IRequestHandler<GetUserProfileDetailQuery, GetUserProfileDetailResponse>
{
    private readonly IAuthenticatedUser _authenticatedUser;
    private readonly ILogger<GetUserProfileDetailQueryHandler> _logger;
    private readonly IDistributedCache _cache;

    public GetUserProfileDetailQueryHandler(
        IApplicationDbContext dbContext,
        IAuthenticatedUser authenticatedUser,
        IDistributedCache cache,
        ILogger<GetUserProfileDetailQueryHandler> logger) : base(dbContext)
    {
        _authenticatedUser = authenticatedUser;
        _cache = cache;
        _logger = logger;
    }

    public async Task<GetUserProfileDetailResponse> Handle(GetUserProfileDetailQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"userProfile:{request.Id}";
        _logger.LogInformation("fetching data for key: {cacheKey} from cache.", cacheKey);

        var userProfile = await _cache.GetOrSetAsync(cacheKey,
            async () =>
            {
                _logger.LogInformation("cache miss. fetching data for key:  {CacheKey} from database.", cacheKey);
                return await _dbContext.UserProfiles
                           .Where(j => j.Id == request.Id && j.UserId == _authenticatedUser.Id)
                           .Select(p => new GetUserProfileDetailResponse
                           {
                               Id = p.Id,
                               Bio = p.Bio,
                               Education = p.Education,
                               Experience = p.Experience,
                               FirstName = p.FirstName,
                               LastName = p.LastName,
                               Location = p.Location,
                               PhoneNumber = p.PhoneNumber,
                           })
                           .FirstOrDefaultAsync(cancellationToken)
                       ?? throw new NotFoundException("Profile not found");
            });

        if (userProfile == null)
        {
            throw new NullDataException("Retrieved data is null");
        }

        return userProfile;
    }
}
