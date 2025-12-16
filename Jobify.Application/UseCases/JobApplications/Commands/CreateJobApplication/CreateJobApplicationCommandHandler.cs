namespace Jobify.Application.UseCases.JobApplications.Commands.CreateJobApplication;

public class CreateJobApplicationCommandHandler : BaseSetting,
    IRequestHandler<CreateJobApplicationCommand, Guid>
{
    private readonly IAuthenticatedUser _authenticatedUser;
    private readonly ILogger<CreateJobApplicationCommandHandler> _logger;

    public CreateJobApplicationCommandHandler(IMapper mapper,
        IApplicationDbContext dbContext,
        IAuthenticatedUser authenticatedUser,
        ILogger<CreateJobApplicationCommandHandler> logger)
        : base(mapper, dbContext)
    {
        _authenticatedUser = authenticatedUser;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateJobApplicationCommand request, CancellationToken cancellationToken)
    {
        var submittedApplication = await _dbContext.JobApplications
            .AnyAsync(a => a.JobListingId == request.JobListingId &&
                           a.ApplicantId == _authenticatedUser.Id, cancellationToken);

        if (submittedApplication)
        {
            throw new DomainException("You have already applied to this job.");
        }

        var jobListing = await _dbContext.JobListings
            .Where(l => l.Id == request.JobListingId)
            .FirstOrDefaultAsync(cancellationToken);

        if (jobListing == null)
        {
            throw new NotFoundException("Job listing not found.");
        }

        var application = _mapper.Map<JobApplication>(request);

        application.ApplicantId = _authenticatedUser.Id;
        application.AppliedAt = DateTimeOffset.UtcNow;

        await _dbContext.JobApplications.AddAsync(application, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"Created job application by user {application.ApplicantId}");

        return application.Id;
    }
}
