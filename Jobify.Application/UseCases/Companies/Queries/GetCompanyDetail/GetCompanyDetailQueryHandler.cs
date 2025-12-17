namespace Jobify.Application.UseCases.Companies.Queries.GetCompanyDetail;

public class GetCompanyDetailQueryHandler : BaseSetting,
    IRequestHandler<GetCompanyDetailQuery, GetCompanyDetailResponse>
{
    private readonly IAuthenticatedUser _authenticatedUser;

    public GetCompanyDetailQueryHandler(IApplicationDbContext dbContext,
        IAuthenticatedUser authenticatedUser) : base(dbContext)
    {
        _authenticatedUser = authenticatedUser;
    }

    public async Task<GetCompanyDetailResponse> Handle(GetCompanyDetailQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Companies
            .AsNoTracking()
            .Where(c => c.Id == request.Id && c.CreatedById == _authenticatedUser.Id)
            .Select(c => new GetCompanyDetailResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description =  c.Description,
                Industry =  c.Industry,
                WebsiteUrl =   c.WebsiteUrl,
            })
            .FirstOrDefaultAsync(cancellationToken) ?? throw new NullDataException("Company not found");
    }
}
