namespace Jobify.Application.UseCases.JobListings.Commands.CreateJobListing;

public class CreateJobListingCommandHandler : BaseSetting, IRequestHandler<CreateJobListingCommand, JobListingDto>
{
    private readonly IAuthenticatedUser _authenticatedUser;
    private readonly IDistributedCache _cache;
    private readonly ILogger<CreateJobListingCommandHandler> _logger;

    public CreateJobListingCommandHandler(
        IApplicationDbContext dbContext,
        IAuthenticatedUser  authenticatedUser,
        ILogger<CreateJobListingCommandHandler> logger) : base(dbContext)
    {
        _authenticatedUser = authenticatedUser;
        _logger = logger;
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

        var jobListing = new JobListing
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Requirements = request.Requirements,
            Location = request.Location,
            Salary = request.Salary,
            Currency =  request.Currency,
            Status = JobStatus.Open,
            CompanyId =  request.CompanyId,
            PostedAt = DateTimeOffset.UtcNow
        };

        await _dbContext.JobListings.AddAsync(jobListing, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new JobListingDto(jobListing.Id);
    }
}
