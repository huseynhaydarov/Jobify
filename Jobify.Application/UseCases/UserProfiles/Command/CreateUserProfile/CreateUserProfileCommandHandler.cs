namespace Jobify.Application.UseCases.UserProfiles.Command.CreateUserProfile;

public class CreateUserProfileCommandHandler : BaseSetting, IRequestHandler<CreateUserProfileCommand, UserProfileDto>
{
    private readonly IAuthenticatedUser _authenticatedUser;

    public CreateUserProfileCommandHandler(
        IApplicationDbContext dbContext,
        IAuthenticatedUser authenticatedUser)
        : base(dbContext)
    {
        _authenticatedUser = authenticatedUser;
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

        return new UserProfileDto(userProfile.Id);
    }
}
