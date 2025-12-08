namespace Jobify.Application.UseCases.JobListings.Commands.CreateJobListing;

public class CreateJobListingCommandValidator : AbstractValidator<CreateJobListingCommand>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IAuthenticatedUser _authenticatedUser;



    public CreateJobListingCommandValidator(IApplicationDbContext dbContext, IAuthenticatedUser authenticatedUser)
    {
        _dbContext = dbContext;
        _authenticatedUser = authenticatedUser;

        RuleFor(x => x.CompanyId)
            .NotEmpty()
            .WithMessage("CompanyId is required.")
            .DependentRules(() =>
            {
                RuleFor(x => x.CompanyId)
                    .MustAsync(CanEmployerPost)
                    .WithMessage("Employer is not eligible to post!");
            });

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(2000);

        RuleFor(x => x.Location)
            .MaximumLength(200);
    }


    private async Task<bool> CanEmployerPost(Guid Id, CancellationToken cancellationToken)
    {
        return await _dbContext.Companies
            .AsNoTracking()
            .Include(c => c.Employers)
            .Select(c => c.Employers
                .Where(e => e.Id == Id))
            .AnyAsync(cancellationToken);
    }
}
