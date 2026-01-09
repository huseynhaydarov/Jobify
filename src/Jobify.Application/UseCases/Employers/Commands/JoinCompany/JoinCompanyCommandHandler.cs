namespace Jobify.Application.UseCases.Employers.Commands.JoinCompany;

public class JoinCompanyCommandHandler : BaseSetting, IRequestHandler<JoinCompanyCommand, Unit>
{
    private readonly IAuthenticatedUser _authenticatedUser;

    public JoinCompanyCommandHandler(IApplicationDbContext dbContext,
        IAuthenticatedUser authenticatedUser) : base(dbContext) =>
        _authenticatedUser = authenticatedUser;

    public async Task<Unit> Handle(JoinCompanyCommand request, CancellationToken cancellationToken)
    {
        User? user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == _authenticatedUser.Id, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        Company? company = await _dbContext.Companies
            .FirstOrDefaultAsync(c => c.Id == request.CompanyId, cancellationToken);

        if (company == null)
        {
            throw new NotFoundException("Company not found");
        }

        Employer? existingEmployer = await _dbContext.Employers
            .FirstOrDefaultAsync(e => e.UserId == _authenticatedUser.Id, cancellationToken);

        if (existingEmployer != null)
        {
            throw new BadRequestException("User is already associated with a company.");
        }

        Employer employer = new() { UserId = user.Id, CompanyId = company.Id };

        await _dbContext.Employers.AddAsync(employer, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
