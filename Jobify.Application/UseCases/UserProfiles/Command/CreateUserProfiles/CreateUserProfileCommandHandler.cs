namespace Jobify.Application.UseCases.UserProfiles.Command.CreateUserProfiles;

public class CreateUserProfileCommandHandler : BaseSetting, IRequestHandler<CreateUserProfileCommand, Unit>
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

    public async Task<Unit> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var userProfile = _mapper.Map<UserProfile>(request);

        userProfile.UserId = _authenticatedUser.Id;

        await _dbContext.UserProfiles.AddAsync(userProfile, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
