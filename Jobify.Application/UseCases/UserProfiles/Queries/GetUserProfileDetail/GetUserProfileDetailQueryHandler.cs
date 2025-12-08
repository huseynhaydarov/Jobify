namespace Jobify.Application.UseCases.UserProfiles.Queries.GetUserProfileDetail;

public class GetUserProfileDetailQueryHandler : BaseSetting, IRequestHandler<GetUserProfileDetailQuery, GetUserProfileDetailVievModel>
{
    private readonly IAuthenticatedUser _authenticatedUser;

    public GetUserProfileDetailQueryHandler(
        IMapper mapper,
        IApplicationDbContext dbContext,
        IAuthenticatedUser authenticatedUser) : base(mapper, dbContext)
    {
        _authenticatedUser = authenticatedUser;
    }

    public async Task<GetUserProfileDetailVievModel> Handle(GetUserProfileDetailQuery request,
        CancellationToken cancellationToken)
    {
        return await _dbContext.UserProfiles
                   .Where(j => j.Id == request.Id && j.UserId == _authenticatedUser.Id)
                   .ProjectTo<GetUserProfileDetailVievModel>(_mapper.ConfigurationProvider)
                   .FirstOrDefaultAsync(cancellationToken)
               ?? throw new NotFoundException("Profile not found");
    }
}
