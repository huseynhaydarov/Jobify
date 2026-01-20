namespace Jobify.Application.UseCases.JobSeekers.Queries.GetJobSeekers;

public class GetAllJobSeekersQueryHandler : BaseSetting,
    IRequestHandler<GetAllJobSeekersQuery, PaginatedResult<GetAllJobSeekersResponse>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetAllJobSeekersQueryHandler(IApplicationDbContext dbContext) : base(dbContext) => _dbContext = dbContext;

    public async Task<PaginatedResult<GetAllJobSeekersResponse>> Handle(GetAllJobSeekersQuery request,
        CancellationToken cancellationToken)
    {
        var queryable =
            _dbContext.Users
                .AsNoTracking()
                .Where(u =>
                    u.UserRoles.Any(ur => ur.Role != null && ur.Role.Name == UserRoles.JobSeeker))
                .OrderByDescending(u => u.CreatedAt)
                .Select(u => new GetAllJobSeekersResponse { Id = u.Id, Email = u.Email });


        return await queryable.PaginatedListAsync(
            request.PagingParameters.PageNumber,
            request.PagingParameters.PageSize,
            cancellationToken);
    }
}
