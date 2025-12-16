namespace Jobify.Application.UseCases.JobSeekers.EventHandlers;

public class EmployerCreatedEventHandler : INotificationHandler<EmployerCreatedEvent>
{
    private readonly ILogger<EmployerCreatedEventHandler> _logger;


    public EmployerCreatedEventHandler(ILogger<EmployerCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(EmployerCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: {DomainEvent}", notification.GetType().Name);

        _logger.LogInformation("New employer user created with ID {UserId}", notification.User.Id);

        await Task.CompletedTask;
    }
}
