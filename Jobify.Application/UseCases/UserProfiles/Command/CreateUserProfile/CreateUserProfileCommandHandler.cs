namespace Jobify.Application.UseCases.UserProfiles.Command.CreateUserProfile;

public class CreateUserProfileCommandHandler : BaseSetting, IRequestHandler<CreateUserProfileCommand, UserProfileDto>
{
    private readonly IAuthenticatedUser _authenticatedUser;

    public CreateUserProfileCommandHandler(
        IMapper mapper,
        IApplicationDbContext dbContext,
        IAuthenticatedUser authenticatedUser)
        : base(mapper, dbContext)
    {
        _authenticatedUser = authenticatedUser;
    }

    public async Task<UserProfileDto> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var userProfile = _mapper.Map<UserProfile>(request);

        userProfile.UserId = _authenticatedUser.Id;

        await _dbContext.UserProfiles.AddAsync(userProfile, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new UserProfileDto(userProfile.Id);
    }
}
