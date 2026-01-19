namespace Jobify.Application.UseCases.JobApplications.Commands.CreateJobApplication;

public class CreateJobApplicationCommandHandler : BaseSetting,
    IRequestHandler<CreateJobApplicationCommand, JobApplicationDto>
{
    private readonly IAuthenticatedUserService _authenticatedUserService;
    private readonly ILogger<CreateJobApplicationCommandHandler> _logger;

    public CreateJobApplicationCommandHandler(
        IApplicationDbContext dbContext,
        IAuthenticatedUserService authenticatedUserService,
        ILogger<CreateJobApplicationCommandHandler> logger)
        : base(dbContext)
    {
        _authenticatedUserService = authenticatedUserService;
        _logger = logger;
    }

    public async Task<JobApplicationDto> Handle(CreateJobApplicationCommand request,
        CancellationToken cancellationToken)
    {
        Guid userId = _authenticatedUserService.Id
                      ?? throw new UnauthorizedException("User is not authenticated");

        bool submittedApplication = await _dbContext.JobApplications
            .AnyAsync(a => a.JobListingId == request.JobListingId &&
                           a.ApplicantId == userId, cancellationToken);

        if (submittedApplication)
        {
            throw new DomainException("You have already applied to this job.");
        }

        JobListing? jobListing = await _dbContext.JobListings
            .Where(l => l.Id == request.JobListingId && !l.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);

        if (jobListing == null)
        {
            throw new NotFoundException("Job listing not found.");
        }

        if (jobListing.Status != JobStatus.Open)
        {
            throw new DomainException("Can apply only to open job.");
        }

        JobApplication application = new()
        {
            JobListingId = request.JobListingId,
            ApplicantId = userId,
            AppliedAt = DateTimeOffset.UtcNow,
            ApplicationStatus = ApplicationStatus.Applied
        };

        await _dbContext.JobApplications.AddAsync(application, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"Created job application by user {application.ApplicantId}");

        return new JobApplicationDto(application.Id, JobStatus.Open);
    }
}
