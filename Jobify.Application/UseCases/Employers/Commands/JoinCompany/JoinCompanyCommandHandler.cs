namespace Jobify.Application.UseCases.Employers.Commands.JoinCompany;

public class JoinCompanyCommandHandler : BaseSetting, IRequestHandler<JoinCompanyCommand, Unit>
{
    private readonly IAuthenticatedUser _authenticatedUser;

    public JoinCompanyCommandHandler(
        IMapper mapper,
        IApplicationDbContext dbContext,
        IAuthenticatedUser authenticatedUser) : base(mapper, dbContext)
    {
        _authenticatedUser = authenticatedUser;
    }

    public async Task<Unit> Handle(JoinCompanyCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == _authenticatedUser.Id, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        var company = await _dbContext.Companies
            .FirstOrDefaultAsync(c => c.Id == request.CompanyId, cancellationToken);

        if (company == null)
        {
            throw new NotFoundException("Company not found");
        }

        var existingEmployer = await _dbContext.Employers
            .FirstOrDefaultAsync(e => e.UserId == _authenticatedUser.Id, cancellationToken);

        if (existingEmployer != null)
        {
            throw new BadRequestException("User is already associated with a company.");
        }

        var employer = _mapper.Map<Employer>(request);

        employer.UserId = user.Id;
        employer.CompanyId = company.Id;
        employer.JoinedAt = DateTimeOffset.UtcNow;

        await _dbContext.Employers.AddAsync(employer, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;

    }
}
