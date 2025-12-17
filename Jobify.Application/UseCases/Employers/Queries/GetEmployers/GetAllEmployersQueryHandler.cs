namespace Jobify.Application.UseCases.Employers.Queries.GetEmployers;

public class GetAllEmployersQueryHandler : BaseSetting,
    IRequestHandler<GetAllEmployersQuery, PaginatedResult<GetAllEmployersResponse>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetAllEmployersQueryHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<PaginatedResult<GetAllEmployersResponse>> Handle(GetAllEmployersQuery request,
        CancellationToken cancellationToken)
    {
        var queryable = _dbContext.Users
            .AsNoTracking()
            .OrderByDescending(c => c.CreatedAt)
            .Select(e => new GetAllEmployersResponse
            {
                Id = e.Id,
                Email = e.Email,
            });

        return await queryable.PaginatedListAsync(
            request.PagingParameters.PageNumber,
            request.PagingParameters.PageSize,
            cancellationToken);
    }
}
