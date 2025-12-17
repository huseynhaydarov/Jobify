namespace Jobify.Application.UseCases.Companies.Commands.UpdateCompanies;

public class UpdateCompanyCommandHandler : BaseSetting, IRequestHandler<UpdateCompanyCommand, Unit>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IAuthenticatedUser _authenticatedUser;

    public UpdateCompanyCommandHandler(IApplicationDbContext dbContext,
        IAuthenticatedUser authenticatedUser) : base(dbContext)
    {
        _dbContext = dbContext;
        _authenticatedUser = authenticatedUser;
    }

    public async Task<Unit> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await _dbContext.Companies
            .FirstOrDefaultAsync(c => c.Id == request.Id && c.CreatedById == _authenticatedUser.Id,
                cancellationToken)
                      ?? throw new NotFoundException("Company not found");

        company.MapFrom(request);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
