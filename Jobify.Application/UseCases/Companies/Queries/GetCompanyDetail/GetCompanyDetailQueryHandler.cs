namespace Jobify.Application.UseCases.Companies.Queries.GetCompanyDetail;

public class GetCompanyDetailQueryHandler : BaseSetting, IRequestHandler<GetCompanyDetailQuery, GetCompanyDetailViewModel>
{
    public GetCompanyDetailQueryHandler(IMapper mapper, IApplicationDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public async Task<GetCompanyDetailViewModel> Handle(GetCompanyDetailQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Companies
            .AsNoTracking()
            .Where(c => c.Id == request.Id)
            .ProjectTo<GetCompanyDetailViewModel>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken) ?? throw new NullDataException("Company not found");
    }
}
