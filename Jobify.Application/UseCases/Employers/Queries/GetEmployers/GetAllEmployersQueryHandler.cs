namespace Jobify.Application.UseCases.Employers.Queries.GetEmployers;

public class GetAllEmployersQueryHandler : BaseSetting,
    IRequestHandler<GetAllEmployersQuery, PaginatedList<GetAllEmployersResponse>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetAllEmployersQueryHandler(IMapper mapper,
        IApplicationDbContext dbContext) : base(mapper,
        dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<PaginatedList<GetAllEmployersResponse>> Handle(GetAllEmployersQuery request,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .OrderByDescending(x => x.CreatedAt)
            .ProjectTo<GetAllEmployersResponse>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PagingParameters.PageNumber, request.PagingParameters.PageSize, cancellationToken);
    }
}
