namespace Jobify.Application.UseCases.UserProfiles.Command.UpdateUserProfile;

public class UpdateUserProfileCommandHandler : BaseSetting, IRequestHandler<UpdateUserProfileCommand, Unit>
{
    private readonly IAuthenticatedUser _authenticatedUser;

    public UpdateUserProfileCommandHandler(
        IApplicationDbContext dbContext,
        IAuthenticatedUser authenticatedUser) : base(dbContext)
    {
        _authenticatedUser = authenticatedUser;
    }

    public async Task<Unit> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _dbContext.UserProfiles
                .Where(c => c.Id == request.Id && c.UserId == _authenticatedUser.Id )
                .FirstOrDefaultAsync(cancellationToken)
                      ?? throw new NotFoundException("Profile not found");

        profile.MapTo(request);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
