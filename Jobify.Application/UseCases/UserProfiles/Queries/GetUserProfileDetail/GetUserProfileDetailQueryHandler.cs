namespace Jobify.Application.UseCases.UserProfiles.Queries.GetUserProfileDetail;

public class GetUserProfileDetailQueryHandler : BaseSetting, IRequestHandler<GetUserProfileDetailQuery, GetUserProfileDetailResponse>
{
    private readonly IAuthenticatedUser _authenticatedUser;

    public GetUserProfileDetailQueryHandler(
        IApplicationDbContext dbContext,
        IAuthenticatedUser authenticatedUser) : base(dbContext)
    {
        _authenticatedUser = authenticatedUser;
    }

    public async Task<GetUserProfileDetailResponse> Handle(GetUserProfileDetailQuery request,
        CancellationToken cancellationToken)
    {
        return await _dbContext.UserProfiles
                   .Where(j => j.Id == request.Id && j.UserId == _authenticatedUser.Id)
                   .Select(p => new GetUserProfileDetailResponse
                   {
                       Id = p.Id,
                       Bio =  p.Bio,
                       Education =  p.Education,
                       Experience =   p.Experience,
                       FirstName = p.FirstName,
                       LastName = p.LastName,
                       Location =   p.Location,
                       PhoneNumber =  p.PhoneNumber,
                   })
                   .FirstOrDefaultAsync(cancellationToken)
               ?? throw new NotFoundException("Profile not found");
    }
}
