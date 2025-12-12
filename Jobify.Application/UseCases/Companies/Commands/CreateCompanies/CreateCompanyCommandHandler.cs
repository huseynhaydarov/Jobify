namespace Jobify.Application.UseCases.Companies.Commands.CreateCompanies;

public class CreateCompanyCommandHandler : BaseSetting, IRequestHandler<CreateCompanyCommand, CompanyDto>
{
    private readonly IAuthenticatedUser _authenticatedUser;

    public CreateCompanyCommandHandler(IMapper mapper,
        IApplicationDbContext dbContext,
        IAuthenticatedUser authenticatedUser) : base(mapper, dbContext)
    {
        _authenticatedUser = authenticatedUser;
    }

    public async Task<CompanyDto> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = _mapper.Map<Company>(request);

        company.CreatedById = _authenticatedUser.Id;

        await _dbContext.Companies.AddAsync(company, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CompanyDto(company.Id);
    }
}
