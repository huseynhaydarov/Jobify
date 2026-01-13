namespace Jobify.Application.UseCases.UserProfiles.Command.UpdateUserProfile;

public class UpdateUserProfileCommandHandler : BaseSetting, IRequestHandler<UpdateUserProfileCommand, Unit>
{
    private readonly IAuthenticatedUser _authenticatedUser;
    private readonly IDistributedCache _cache;
    private readonly ILogger<UpdateUserProfileCommandHandler> _logger;

    public UpdateUserProfileCommandHandler(
        IApplicationDbContext dbContext,
        IAuthenticatedUser authenticatedUser,
        IDistributedCache cache, ILogger<UpdateUserProfileCommandHandler> logger)
        : base(dbContext)
    {
        _authenticatedUser = authenticatedUser;
        _cache = cache;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        UserProfile profile = await _dbContext.UserProfiles
                                  .Where(c => c.Id == request.Id && c.UserId == _authenticatedUser.Id)
                                  .FirstOrDefaultAsync(cancellationToken)
                              ?? throw new NotFoundException("Profile not found");

        profile.MapTo(request);

        await _dbContext.SaveChangesAsync(cancellationToken);

        string cacheKey = $"userProfile:{request.Id}";
        ;
        _logger.LogInformation("invalidating cache for key: {CacheKey} from cache.", cacheKey);
        await _cache.RemoveAsync(cacheKey, cancellationToken);

        return Unit.Value;
    }
}
