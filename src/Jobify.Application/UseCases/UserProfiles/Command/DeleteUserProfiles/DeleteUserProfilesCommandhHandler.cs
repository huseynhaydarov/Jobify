namespace Jobify.Application.UseCases.UserProfiles.Command.DeleteUserProfiles;

public class DeleteUserProfilesCommandhHandler : IRequestHandler<DeleteUserProfilesCommand, Unit>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteUserProfilesCommandhHandler(IApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<Unit> Handle(DeleteUserProfilesCommand request, CancellationToken cancellationToken)
    {
        if (request.Ids.Count == 0)
        {
            return Unit.Value;
        }

        List<UserProfile> profiles = await _dbContext.UserProfiles
            .Where(p => request.Ids.Contains(p.Id))
            .ToListAsync(cancellationToken);

        foreach (UserProfile profile in profiles)
        {
            profile.IsDeleted = true;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
