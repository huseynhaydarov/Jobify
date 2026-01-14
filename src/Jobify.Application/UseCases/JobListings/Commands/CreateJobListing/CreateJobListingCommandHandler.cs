namespace Jobify.Application.UseCases.JobListings.Commands.CreateJobListing;

public class CreateJobListingCommandHandler : BaseSetting, IRequestHandler<CreateJobListingCommand, JobListingDto>
{
    private readonly IAuthenticatedUserService _authenticatedUserService;
    private readonly ILogger<CreateJobListingCommandHandler> _logger;

    public CreateJobListingCommandHandler(
        IApplicationDbContext dbContext,
        IAuthenticatedUserService authenticatedUserService,
        ILogger<CreateJobListingCommandHandler> logger) : base(dbContext)
    {
        _authenticatedUserService = authenticatedUserService;
        _logger = logger;
    }

    public async Task<JobListingDto> Handle(CreateJobListingCommand request, CancellationToken cancellationToken)
    {
        Guid companyId = await _dbContext.Companies
            .Where(c => c.Employers.Any(e => e.UserId == _authenticatedUserService.Id))
            .Select(c => c.Id)
            .FirstOrDefaultAsync(cancellationToken);

        var employer = await _dbContext.Employers
                           .Select(e => new { e.Id, e.CompanyId })
                           .FirstOrDefaultAsync(cancellationToken)
                       ?? throw new NotFoundException("Employer not found");

        if (employer.CompanyId != companyId)
        {
            throw new DomainException("Employer is not eligible to post!");
        }

        JobListing jobListing = new()
        {
            Name = request.Name,
            Description = request.Description,
            Requirements = request.Requirements,
            Location = request.Location,
            Salary = request.Salary,
            Currency = request.Currency,
            Status = JobStatus.Open,
            CompanyId = companyId,
            EmployerId = employer.Id,
            PostedAt = DateTimeOffset.UtcNow,
            ExpiresAt = request.ExpireDate
        };

        await _dbContext.JobListings.AddAsync(jobListing, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new JobListingDto(
            jobListing.Id,
            jobListing.Status,
            jobListing.PostedAt);
    }
}
