namespace Jobify.Application.UseCases.JobSeekers.Queries.GetJobSeekers;

public class GetAllJobSeekersQueryHandler : BaseSetting,
    IRequestHandler<GetAllJobSeekersQuery, PaginatedList<GetAllJobSeekersResponse>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetAllJobSeekersQueryHandler(IMapper mapper,
        IApplicationDbContext dbContext) : base(mapper,
        dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<PaginatedList<GetAllJobSeekersResponse>> Handle(GetAllJobSeekersQuery request,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .OrderByDescending(x => x.CreatedAt)
            .ProjectTo<GetAllJobSeekersResponse>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PagingParameters.PageNumber, request.PagingParameters.PageSize, cancellationToken);
    }
}
