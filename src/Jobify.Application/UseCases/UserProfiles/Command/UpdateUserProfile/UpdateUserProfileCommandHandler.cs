namespace Jobify.Application.UseCases.UserProfiles.Command.UpdateUserProfile;

public class UpdateUserProfileCommandHandler : BaseSetting, IRequestHandler<UpdateUserProfileCommand, Unit>
{
    private readonly IAuthenticatedUserService _authenticatedUserService;
    private readonly IDistributedCache _cache;
    private readonly ILogger<UpdateUserProfileCommandHandler> _logger;

    public UpdateUserProfileCommandHandler(
        IApplicationDbContext dbContext,
        IAuthenticatedUserService authenticatedUserService,
        IDistributedCache cache, ILogger<UpdateUserProfileCommandHandler> logger)
        : base(dbContext)
    {
        _authenticatedUserService = authenticatedUserService;
        _cache = cache;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _dbContext.UserProfiles
                          .Where(c => c.Id == request.Id && c.UserId == _authenticatedUserService.Id)
                          .FirstOrDefaultAsync(cancellationToken)
                      ?? throw new NotFoundException("Profile not found");

        profile.MapTo(request);

        await _dbContext.SaveChangesAsync(cancellationToken);

        var cacheKey = $"userProfile:{request.Id}";
        ;
        _logger.LogInformation("invalidating cache for key: {CacheKey} from cache.", cacheKey);
        await _cache.RemoveAsync(cacheKey, cancellationToken);

        return Unit.Value;
    }
}
