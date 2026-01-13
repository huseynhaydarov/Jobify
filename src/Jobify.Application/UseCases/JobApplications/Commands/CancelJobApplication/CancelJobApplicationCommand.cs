namespace Jobify.Application.UseCases.JobApplications.Commands.CancelJobApplication;

public record CancelJobApplicationCommand(Guid ApplicationId) : IRequest<Unit>;
