namespace Jobify.Application.UseCases.Companies.Commands.CreateCompanies;

public class CreateCompanyCommandHandler : BaseSetting, IRequestHandler<CreateCompanyCommand, CompanyDto>
{
    private readonly IAuthenticatedUserService _authenticatedUserService;

    public CreateCompanyCommandHandler(IApplicationDbContext dbContext,
        IAuthenticatedUserService authenticatedUserService) : base(dbContext) =>
        _authenticatedUserService = authenticatedUserService;

    public async Task<CompanyDto> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        Guid userId = _authenticatedUserService.Id
                      ?? throw new UnauthorizedException("User is not authenticated");

        Company company = new()
        {
            Name = request.Name,
            Description = request.Description,
            WebsiteUrl = request.WebsiteUrl,
            Industry = request.Industry,
            CreatedById = userId
        };

        await _dbContext.Companies.AddAsync(company, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CompanyDto(company.Id);
    }
}
