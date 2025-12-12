namespace Jobify.Application.UseCases.UserProfiles.Queries.GetUserProfileDetail;

public class GetUserProfileDetailQueryHandler : BaseSetting, IRequestHandler<GetUserProfileDetailQuery, GetUserProfileDetailResponse>
{
    private readonly IAuthenticatedUser _authenticatedUser;

    public GetUserProfileDetailQueryHandler(
        IMapper mapper,
        IApplicationDbContext dbContext,
        IAuthenticatedUser authenticatedUser) : base(mapper, dbContext)
    {
        _authenticatedUser = authenticatedUser;
    }

    public async Task<GetUserProfileDetailResponse> Handle(GetUserProfileDetailQuery request,
        CancellationToken cancellationToken)
    {
        return await _dbContext.UserProfiles
                   .Where(j => j.Id == request.Id && j.UserId == _authenticatedUser.Id)
                   .ProjectTo<GetUserProfileDetailResponse>(_mapper.ConfigurationProvider)
                   .FirstOrDefaultAsync(cancellationToken)
               ?? throw new NotFoundException("Profile not found");
    }
}
