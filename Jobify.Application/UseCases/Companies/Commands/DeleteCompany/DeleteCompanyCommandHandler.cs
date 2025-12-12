namespace Jobify.Application.UseCases.Companies.Commands.DeleteCompany;

public class DeleteCompanyCommandHandler : BaseSetting, IRequestHandler<DeleteCompanyCommand, Unit>
{
    private readonly IAuthenticatedUser _authenticatedUser;

    public DeleteCompanyCommandHandler(
        IMapper mapper,
        IApplicationDbContext dbContext,
        IAuthenticatedUser authenticatedUser) : base(mapper, dbContext)
    {
        _authenticatedUser = authenticatedUser;
    }

    public async Task<Unit> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await _dbContext.Companies
                             .Where(x => x.Id == request.CompanyId
                                         && x.CreatedById == _authenticatedUser.Id)
                             .FirstOrDefaultAsync(cancellationToken)
                         ?? throw new NotFoundException("Company not found" );

         company.IsDeleted = true;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
