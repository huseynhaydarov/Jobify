namespace Jobify.Application.UseCases.Companies.Commands.DeleteCompany;

public class DeleteCompanyCommandHandler : BaseSetting, IRequestHandler<DeleteCompanyCommand, Unit>
{
    private readonly IAuthenticatedUserService _authenticatedUserService;

    public DeleteCompanyCommandHandler(IApplicationDbContext dbContext,
        IAuthenticatedUserService authenticatedUserService) : base(dbContext) =>
        _authenticatedUserService = authenticatedUserService;

    public async Task<Unit> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
    {
        Company company = await _dbContext.Companies
                              .Where(x => x.Id == request.CompanyId
                                          && x.CreatedById == _authenticatedUserService.Id)
                              .FirstOrDefaultAsync(cancellationToken)
                          ?? throw new NotFoundException("Company not found");

        company.IsDeleted = true;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
