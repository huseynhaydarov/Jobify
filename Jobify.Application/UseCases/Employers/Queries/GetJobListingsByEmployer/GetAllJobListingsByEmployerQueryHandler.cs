namespace Jobify.Application.UseCases.Employers.Queries.GetJobListingsByEmployer;

public class GetAllJobListingsByEmployerQueryHandler : BaseSetting,
    IRequestHandler<GetAllJobListingsByEmployerQuery, PaginatedResult<GetAllJobListingsByEmployerResponse>>
{
    private readonly IAuthenticatedUser _authenticatedUser;

    public GetAllJobListingsByEmployerQueryHandler(
        IApplicationDbContext dbContext,
        IAuthenticatedUser authenticatedUser)
        : base(dbContext)
    {
        _authenticatedUser = authenticatedUser;
    }

    public async Task<PaginatedResult<GetAllJobListingsByEmployerResponse>> Handle(GetAllJobListingsByEmployerQuery request,
        CancellationToken cancellationToken)
    {
        var employerCompanyId = await _dbContext.Employers
            .AsNoTracking()
            .Where(c => c.UserId == _authenticatedUser.Id)
            .Select(e => e.CompanyId)
            .FirstOrDefaultAsync(cancellationToken);

        var queryable = _dbContext.JobListings
            .IgnoreQueryFilters()
            .Where(c => c.CompanyId == employerCompanyId)
            .AsNoTracking()
            .Select(c => new GetAllJobListingsByEmployerResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Requirements = c.Requirements,
                Location = c.Location,
                Salary = c.Salary,
                Currency = c.Currency,
                Status = c.Status,
                PostedAt = c.PostedAt,
                Views = c.Views,
            });

        return await queryable.PaginatedListAsync(
            request.Parameters.PageNumber,
            request.Parameters.PageSize,
            cancellationToken);
    }
}
