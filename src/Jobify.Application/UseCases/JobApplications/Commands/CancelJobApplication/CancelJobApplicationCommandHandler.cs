namespace Jobify.Application.UseCases.JobApplications.Commands.CancelJobApplication;

public class CancelJobApplicationCommandHandler : IRequestHandler<CancelJobApplicationCommand, CancelJobApplicationResponse>
{
    private readonly IAuthenticatedUser _authenticatedUser;
    private readonly IApplicationDbContext _dbContext;
    private readonly ILogger<CancelJobApplicationCommandHandler> _logger;

    public CancelJobApplicationCommandHandler(IApplicationDbContext dbContext,
        IAuthenticatedUser authenticatedUser,
        ILogger<CancelJobApplicationCommandHandler> logger)
    {
        _dbContext = dbContext;
        _authenticatedUser = authenticatedUser;
        _logger = logger;
    }

    public async Task<CancelJobApplicationResponse> Handle(CancelJobApplicationCommand request, CancellationToken cancellationToken)
    {
        JobApplication? application = await _dbContext.JobApplications
            .FirstOrDefaultAsync(x => x.Id == request.ApplicationId, cancellationToken);

        if (application == null)
        {
            throw new NotFoundException("Application not found");
        }

        if (application.ApplicantId != _authenticatedUser.Id)
        {
            throw new ForbiddenAccessException("You can only cancel your own application");
        }

        if (application.ApplicationStatus != ApplicationStatus.Applied)
        {
            throw new DomainException("Only a submitted application can be cancelled");
        }

        application.ApplicationStatus = ApplicationStatus.Withdrawn;
        application.WithdrawnAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"Cancelled job application by user {application.ApplicantId}");

        return new  CancelJobApplicationResponse(application.Id, application.WithdrawnAt);
    }
}
