namespace Jobify.Application.UseCases.UserProfiles.Command.CreateUserProfile;

public class CreateUserProfileCommandHandler : BaseSetting, IRequestHandler<CreateUserProfileCommand, UserProfileDto>
{
    private readonly IAuthenticatedUser _authenticatedUser;
    private readonly ILogger<CreateUserProfileCommandHandler> _logger;

    public CreateUserProfileCommandHandler(
        IApplicationDbContext dbContext,
        IAuthenticatedUser authenticatedUser,
        ILogger<CreateUserProfileCommandHandler> logger)
        : base(dbContext)
    {
        _authenticatedUser = authenticatedUser;
        _logger = logger;
    }

    public async Task<UserProfileDto> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
    {
        Guid userId = _authenticatedUser.Id
                      ?? throw new UnauthorizedException("User is not authenticated");

        UserProfile userProfile = new()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            Location = request.Location,
            Bio = request.Bio,
            Education = request.Education,
            Experience = request.Experience
        };

        await _dbContext.UserProfiles.AddAsync(userProfile, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new UserProfileDto(userProfile.Id);
    }
}
