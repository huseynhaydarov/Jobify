namespace Jobify.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest>
    where TRequest : notnull
{
    private readonly IAuthenticatedUserService _authenticatedUserService;
    private readonly ILogger _logger;

    public LoggingBehaviour(ILogger<TRequest> logger, IAuthenticatedUserService authenticatedUserService)
    {
        _logger = logger;
        _authenticatedUserService = authenticatedUserService;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = _authenticatedUserService.Id;

        _logger.LogInformation("Request: {Name} {@UserId} {@UserName}",
            requestName, userId, request);
    }
}
