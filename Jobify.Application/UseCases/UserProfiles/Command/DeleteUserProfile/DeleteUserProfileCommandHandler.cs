namespace Jobify.Application.UseCases.UserProfiles.Command.DeleteUserProfile;

public class DeleteUserProfileCommandHandler : BaseSetting, IRequestHandler<DeleteUserProfileCommand, Unit>
{
    private readonly IAuthenticatedUser _authenticatedUser;
    private readonly ILogger<DeleteUserProfileCommandHandler> _logger;
    private readonly IDistributedCache _cache;


    public DeleteUserProfileCommandHandler(
        IApplicationDbContext dbContext,
        IAuthenticatedUser authenticatedUser,
        ILogger<DeleteUserProfileCommandHandler> logger,
        IDistributedCache cache) : base(dbContext)
    {
        _authenticatedUser = authenticatedUser;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteUserProfileCommand request, CancellationToken cancellationToken)
    {
        var userProfile = await _dbContext.UserProfiles
            .Where(u => u.Id == request.Id && u.UserId == _authenticatedUser.Id)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException("Profile not found");

        userProfile.IsDeleted = true;

        await _dbContext.SaveChangesAsync(cancellationToken);

        var cacheKey = $"userProfile:{request.Id}";
        _logger.LogInformation("invalidating cache for key: {CacheKey} from cache.", cacheKey);
        await _cache.RemoveAsync(cacheKey, cancellationToken);

        return Unit.Value;
    }
}
