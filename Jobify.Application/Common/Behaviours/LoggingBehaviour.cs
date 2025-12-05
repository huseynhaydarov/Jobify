namespace Jobify.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest>
    where TRequest : notnull
{
    private readonly ILogger _logger;
    private readonly IAuthenticatedUser _authenticatedUser;

    public LoggingBehaviour(ILogger<TRequest> logger, IAuthenticatedUser authenticatedUser)
    {
        _logger = logger;
        _authenticatedUser = authenticatedUser;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = _authenticatedUser.Id;

        _logger.LogInformation("Request: {Name} {@UserId} {@UserName}",
            requestName, userId, request);
    }
}

