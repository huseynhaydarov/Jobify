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
        var userId = _authenticatedUser.Id
                     ?? throw new UnauthorizedException("User is not authenticated");

        var company = new Company
        {
            Name = request.Name,
            Description = request.Description,
            WebsiteUrl =  request.WebsiteUrl,
            Industry =  request.Industry,
            CreatedById = userId,
        };

        await _dbContext.Companies.AddAsync(company, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CompanyDto(company.Id);
    }
}
