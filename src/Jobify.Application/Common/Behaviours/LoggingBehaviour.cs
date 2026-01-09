namespace Jobify.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest>
    where TRequest : notnull
{
    private readonly IAuthenticatedUser _authenticatedUser;
    private readonly ILogger _logger;

    public LoggingBehaviour(ILogger<TRequest> logger, IAuthenticatedUser authenticatedUser)
    {
        _logger = logger;
        _authenticatedUser = authenticatedUser;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;
        Guid? userId = _authenticatedUser.Id;

        _logger.LogInformation("Request: {Name} {@UserId} {@UserName}",
            requestName, userId, request);
    }
}
