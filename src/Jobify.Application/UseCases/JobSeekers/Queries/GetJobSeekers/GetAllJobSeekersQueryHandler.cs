namespace Jobify.Application.UseCases.JobSeekers.Queries.GetJobSeekers;

public class GetAllJobSeekersQueryHandler : BaseSetting,
    IRequestHandler<GetAllJobSeekersQuery, PaginatedResult<GetAllJobSeekersResponse>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetAllJobSeekersQueryHandler(IApplicationDbContext dbContext) : base(dbContext) => _dbContext = dbContext;

    public async Task<PaginatedResult<GetAllJobSeekersResponse>> Handle(GetAllJobSeekersQuery request,
        CancellationToken cancellationToken)
    {
        IQueryable<GetAllJobSeekersResponse> queryable = _dbContext.Users
            .OrderByDescending(x => x.CreatedAt)
            .Select(c => new GetAllJobSeekersResponse { Id = c.Id, Email = c.Email });

        return await queryable.PaginatedListAsync(
            request.PagingParameters.PageNumber,
            request.PagingParameters.PageSize,
            cancellationToken);
    }
}
