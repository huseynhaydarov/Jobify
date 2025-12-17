namespace Jobify.Application.UseCases.Companies.Commands.CreateCompanies;

public class CreateCompanyCommandHandler : BaseSetting, IRequestHandler<CreateCompanyCommand, CompanyDto>
{
    private readonly IAuthenticatedUser _authenticatedUser;

    public CreateCompanyCommandHandler(IApplicationDbContext dbContext,
        IAuthenticatedUser authenticatedUser) : base(dbContext)
    {
        _authenticatedUser = authenticatedUser;
    }

    public async Task<CompanyDto> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = new Company
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            WebsiteUrl =  request.WebsiteUrl,
            Industry =  request.Industry,
            CreatedById = _authenticatedUser.Id,
        };

        await _dbContext.Companies.AddAsync(company, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CompanyDto(company.Id);
    }
}
