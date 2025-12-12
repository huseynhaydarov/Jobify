namespace Jobify.Application.UseCases.Companies.Queries.GetCompanyDetail;

public class GetCompanyDetailQueryHandler : BaseSetting, IRequestHandler<GetCompanyDetailQuery, GetCompanyDetailResponse>
{
    private readonly IAuthenticatedUser _authenticatedUser;

    public GetCompanyDetailQueryHandler(
        IMapper mapper,
        IApplicationDbContext dbContext,
        IAuthenticatedUser authenticatedUser) : base(mapper, dbContext)
    {
        _authenticatedUser = authenticatedUser;
    }

    public async Task<GetCompanyDetailResponse> Handle(GetCompanyDetailQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Companies
            .AsNoTracking()
            .Where(c => c.Id == request.Id && c.CreatedById == _authenticatedUser.Id)
            .ProjectTo<GetCompanyDetailResponse>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken) ?? throw new NullDataException("Company not found");
    }
}
