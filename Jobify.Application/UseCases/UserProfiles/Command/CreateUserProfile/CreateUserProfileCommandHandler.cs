namespace Jobify.Application.UseCases.UserProfiles.Command.CreateUserProfile;

public class CreateUserProfileCommandHandler : BaseSetting, IRequestHandler<CreateUserProfileCommand, UserProfileDto>
{
    private readonly IAuthenticatedUser _authenticatedUser;
    private readonly ILogger<CreateUserProfileCommandHandler> _logger;
    private readonly IDistributedCache _cache;

    public CreateUserProfileCommandHandler(
        IApplicationDbContext dbContext,
        IAuthenticatedUser authenticatedUser,
        ILogger<CreateUserProfileCommandHandler> logger,
        IDistributedCache cache)
        : base(dbContext)
    {
        _authenticatedUser = authenticatedUser;
        _logger = logger;
        _cache = cache;
    }

    public async Task<UserProfileDto> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
    {

        var userProfile = new UserProfile
        {
            Id = Guid.NewGuid(),
            UserId = _authenticatedUser.Id,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            Location = request.Location,
            Bio = request.Bio,
            Education = request.Education,
            Experience = request.Experience,
        };

        await _dbContext.UserProfiles.AddAsync(userProfile, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var cacheKey = "userProfiles";
        _logger.LogInformation("invalidating cache for key: {CacheKey} from cache.", cacheKey);
        await _cache.RemoveAsync(cacheKey, cancellationToken);

        return new UserProfileDto(userProfile.Id);
    }
}
