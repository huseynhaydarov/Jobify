namespace Jobify.Application.UseCases.Companies.Commands.UpdateCompanies;

public class UpdateCompanyCommandHandler : BaseSetting, IRequestHandler<UpdateCompanyCommand, Unit>
{
    private readonly IAuthenticatedUserService _authenticatedUserService;
    private readonly IApplicationDbContext _dbContext;

    public UpdateCompanyCommandHandler(IApplicationDbContext dbContext,
        IAuthenticatedUserService authenticatedUserService) : base(dbContext)
    {
        _dbContext = dbContext;
        _authenticatedUserService = authenticatedUserService;
    }

    public async Task<Unit> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await _dbContext.Companies
                          .FirstOrDefaultAsync(c => c.Id == request.Id && c.CreatedById == _authenticatedUserService.Id,
                              cancellationToken)
                      ?? throw new NotFoundException("Company not found");

        company.MapFrom(request);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
