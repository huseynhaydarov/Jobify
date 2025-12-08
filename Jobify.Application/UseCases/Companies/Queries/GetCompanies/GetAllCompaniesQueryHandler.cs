namespace Jobify.Application.UseCases.Companies.Queries.GetCompanies;

public class GetAllCompaniesQueryHandler : BaseSetting, IRequestHandler<GetAllCompaniesQuery, PaginatedList<GetAllCompaniesViewModel>>
{
    public GetAllCompaniesQueryHandler(IMapper mapper, IApplicationDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public async Task<PaginatedList<GetAllCompaniesViewModel>> Handle(GetAllCompaniesQuery request,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Companies
            .AsNoTracking()
            .ProjectTo<GetAllCompaniesViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Parameters.PageNumber, request.Parameters.PageSize);
    }
}
