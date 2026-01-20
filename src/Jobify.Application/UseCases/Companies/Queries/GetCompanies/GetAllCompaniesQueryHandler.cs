namespace Jobify.Application.UseCases.Companies.Queries.GetCompanies;

public class GetAllCompaniesQueryHandler : BaseSetting,
    IRequestHandler<GetAllCompaniesQuery, PaginatedResult<GetAllCompaniesResponse>>
{
    public GetAllCompaniesQueryHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<PaginatedResult<GetAllCompaniesResponse>> Handle(GetAllCompaniesQuery request,
        CancellationToken cancellationToken)
    {
        var queryable = _dbContext.Companies
            .AsNoTracking()
            .Select(c => new GetAllCompaniesResponse
            {
                Id = c.Id,
                Name = c.Name,
                WebsiteUrl = c.WebsiteUrl,
                Description = c.Description,
                Industry = c.Industry
            });

        return await queryable.PaginatedListAsync(
            request.Parameters.PageNumber,
            request.Parameters.PageSize,
            cancellationToken);
    }
}
