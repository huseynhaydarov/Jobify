namespace Jobify.Application.UseCases.Employers.Queries.GetEmployers;

public class GetAllEmployersQueryHandler : BaseSetting,
    IRequestHandler<GetAllEmployersQuery, PaginatedResult<GetAllEmployersResponse>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetAllEmployersQueryHandler(IApplicationDbContext dbContext) : base(dbContext) => _dbContext = dbContext;

    public async Task<PaginatedResult<GetAllEmployersResponse>> Handle(GetAllEmployersQuery request,
        CancellationToken cancellationToken)
    {
        IQueryable<GetAllEmployersResponse> queryable =
            _dbContext.Users
                .AsNoTracking()
                .Where(u =>
                    u.UserRoles.Any(ur => ur.Role != null && ur.Role.Name == UserRoles.Employer))
                .OrderByDescending(u => u.CreatedAt)
                .Select(u => new GetAllEmployersResponse
                {
                    Id = u.Id,
                    Email = u.Email
                });

        return await queryable.PaginatedListAsync(
            request.PagingParameters.PageNumber,
            request.PagingParameters.PageSize,
            cancellationToken);
    }
}
