namespace Jobify.Application.UseCases.Companies.Queries.GetCompanyDetail;

public class GetCompanyDetailQueryHandler : BaseSetting,
    IRequestHandler<GetCompanyDetailQuery, GetCompanyDetailResponse>
{
    private readonly IAuthenticatedUserService _authenticatedUserService;

    public GetCompanyDetailQueryHandler(IApplicationDbContext dbContext,
        IAuthenticatedUserService authenticatedUserService) : base(dbContext) =>
        _authenticatedUserService = authenticatedUserService;

    public async Task<GetCompanyDetailResponse> Handle(GetCompanyDetailQuery request,
        CancellationToken cancellationToken) =>
        await _dbContext.Companies
            .AsNoTracking()
            .Where(c => c.Id == request.Id && c.CreatedById == _authenticatedUserService.Id)
            .Select(c => new GetCompanyDetailResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Industry = c.Industry,
                WebsiteUrl = c.WebsiteUrl
            })
            .FirstOrDefaultAsync(cancellationToken) ?? throw new NullDataException("Company not found");
}
