namespace Jobify.Application.UseCases.JobApplications.Commands.UpdateJobApplicationStatus;

public class ApplicationStatusUpdateCommandHandler
    : IRequestHandler<ApplicationStatusUpdateCommand, Unit>
{
    private readonly IAuthenticatedUser _authenticatedUser;
    private readonly IApplicationDbContext _dbContext;

    public ApplicationStatusUpdateCommandHandler(
        IApplicationDbContext dbContext,
        IAuthenticatedUser authenticatedUser)
    {
        _dbContext = dbContext;
        _authenticatedUser = authenticatedUser;
    }

    public async Task<Unit> Handle(ApplicationStatusUpdateCommand request, CancellationToken cancellationToken)
    {
        JobApplication? application = await _dbContext.JobApplications
            .Where(x => x.Id == request.applicationId
                        && x.ApplicationStatus != (ApplicationStatus)request.status)
            .FirstOrDefaultAsync(cancellationToken);

        if (application is null)
        {
            throw new NotFoundException("Job application not found.");
        }

        ApplicationStatus newStatus = (ApplicationStatus)request.status;

        ValidateStatusTransition(application.ApplicationStatus, newStatus);

        application.ApplicationStatus = newStatus;

        if (newStatus == ApplicationStatus.Withdrawn)
        {
            application.WithdrawnAt = DateTimeOffset.UtcNow;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private static void ValidateStatusTransition(
        ApplicationStatus current,
        ApplicationStatus next)
    {
        if (current is ApplicationStatus.Rejected or ApplicationStatus.Hired)
        {
            throw new DomainException("Finalized applications cannot be updated.");
        }

        if (current == ApplicationStatus.Withdrawn)
        {
            throw new DomainException("Withdrawn applications cannot be updated.");
        }

        if (current == ApplicationStatus.Applied && next == ApplicationStatus.Hired)
        {
            throw new DomainException("Application must go through interview process first.");
        }
    }
}
