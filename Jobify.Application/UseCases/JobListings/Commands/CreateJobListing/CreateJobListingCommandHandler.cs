namespace Jobify.Application.UseCases.JobListings.Commands.CreateJobListing;

public class CreateJobListingCommandHandler : BaseSetting, IRequestHandler<CreateJobListingCommand, Guid>
{
    private readonly IAuthenticatedUser _authenticatedUser;

    public CreateJobListingCommandHandler(
        IApplicationDbContext dbContext,
        IMapper mapper,
        IAuthenticatedUser  authenticatedUser) : base(mapper, dbContext)
    {
        _authenticatedUser = authenticatedUser;
    }

    public async Task<Guid> Handle(CreateJobListingCommand request, CancellationToken cancellationToken)
    {
        var employer = await _dbContext.Employers
            .FirstOrDefaultAsync(e => e.UserId == _authenticatedUser.Id, cancellationToken)
            ?? throw new NotFoundException("Employer not found.");

        var jobListing = _mapper.Map<JobListing>(request);

        jobListing.Status = JobStatus.Open;
        jobListing.EmployerId = employer.Id;
        jobListing.CompanyId = request.CompanyId;

        await _dbContext.JobListings.AddAsync(jobListing, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return jobListing.Id;
    }
}
