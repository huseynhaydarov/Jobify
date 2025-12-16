namespace Jobify.Application.UseCases.JobListings.Commands.CreateJobListing;

public class CreateJobListingCommandHandler : BaseSetting, IRequestHandler<CreateJobListingCommand, JobListingDto>
{
    private readonly IAuthenticatedUser _authenticatedUser;

    public CreateJobListingCommandHandler(
        IApplicationDbContext dbContext,
        IMapper mapper,
        IAuthenticatedUser  authenticatedUser) : base(mapper, dbContext)
    {
        _authenticatedUser = authenticatedUser;
    }

    public async Task<JobListingDto> Handle(CreateJobListingCommand request, CancellationToken cancellationToken)
    {
        var employer = await _dbContext.Employers
            .FirstOrDefaultAsync(e => e.UserId != _authenticatedUser.Id, cancellationToken)
            ?? throw new NotFoundException("Employer not found.");

        if (employer.CompanyId != request.CompanyId)
        {
            throw new DomainException("Employer is not eligible to post!");
        }

        var jobListing = _mapper.Map<JobListing>(request);

        jobListing.PostedAt = DateTimeOffset.Now;
        jobListing.Status = JobStatus.Open;
        jobListing.EmployerId = employer.Id;
        jobListing.CompanyId = request.CompanyId;

        await _dbContext.JobListings.AddAsync(jobListing, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new JobListingDto(jobListing.Id);
    }
}
