namespace Jobify.Application.UseCases.UserProfiles.Queries.GetUserProfiles;

public class GetAllUserProfilesQueryHandler : BaseSetting, IRequestHandler<GetAllUserProfilesQuery,
    PaginatedList<GetAllUserProfilesResponse>>
{
    public GetAllUserProfilesQueryHandler(IMapper mapper, IApplicationDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public async Task<PaginatedList<GetAllUserProfilesResponse>> Handle(GetAllUserProfilesQuery request,
        CancellationToken cancellationToken)
    {
        return await _dbContext.UserProfiles
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .ProjectTo<GetAllUserProfilesResponse>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PagingParameters.PageNumber, request.PagingParameters.PageSize, cancellationToken);
    }
}
