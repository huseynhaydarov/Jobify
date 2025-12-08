namespace Jobify.Application.UseCases.UserProfiles.Queries.GetUserProfiles;

public class GetAllUserProfilesQueryHandler : BaseSetting, IRequestHandler<GetAllUserProfilesQuery,
    PaginatedList<GetAllUserProfilesViewModel>>
{
    public GetAllUserProfilesQueryHandler(IMapper mapper, IApplicationDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public async Task<PaginatedList<GetAllUserProfilesViewModel>> Handle(GetAllUserProfilesQuery request,
        CancellationToken cancellationToken)
    {
        return await _dbContext.UserProfiles
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .ProjectTo<GetAllUserProfilesViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PagingParameters.PageNumber, request.PagingParameters.PageSize, cancellationToken);
    }
}
