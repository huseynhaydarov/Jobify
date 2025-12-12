namespace Jobify.Application.UseCases.UserProfiles.Command.DeleteUserProfile;

public class DeleteUserProfileCommandHandler : BaseSetting, IRequestHandler<DeleteUserProfileCommand, Unit>
{
    private readonly IAuthenticatedUser _authenticatedUser;

    public DeleteUserProfileCommandHandler(
        IMapper mapper,
        IApplicationDbContext dbContext,
        IAuthenticatedUser authenticatedUser) : base(mapper, dbContext)
    {
        _authenticatedUser = authenticatedUser;
    }

    public async Task<Unit> Handle(DeleteUserProfileCommand request, CancellationToken cancellationToken)
    {
        var userProfile = await _dbContext.UserProfiles
            .Where(u => u.Id == request.Id && u.UserId == _authenticatedUser.Id)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException("Profile not found");

        userProfile.IsDeleted = true;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
