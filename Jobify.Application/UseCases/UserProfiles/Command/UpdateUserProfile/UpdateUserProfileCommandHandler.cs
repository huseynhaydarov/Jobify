namespace Jobify.Application.UseCases.UserProfiles.Command.UpdateUserProfile;

public class UpdateUserProfileCommandHandler : BaseSetting, IRequestHandler<UpdateUserProfileCommand, Unit>
{
    public UpdateUserProfileCommandHandler(
        IMapper mapper,
        IApplicationDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public async Task<Unit> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _dbContext.UserProfiles
                .Where(c => c.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken)
                      ?? throw new NotFoundException("Profile not found");

        _mapper.Map(request, profile);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
