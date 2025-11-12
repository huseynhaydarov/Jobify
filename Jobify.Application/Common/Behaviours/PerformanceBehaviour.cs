namespace Jobify.Application.Common.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly Stopwatch _timer;
    private readonly ILogger<TRequest> _logger;
    private readonly IAuthenticatedUser _authenticatedUser;

    public PerformanceBehaviour(
        ILogger<TRequest> logger,
        IAuthenticatedUser authenticatedUser
        )
    {
        _timer = new Stopwatch();

        _logger = logger;
        _authenticatedUser = authenticatedUser;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _timer.Start();

        var response = await next();

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > 500)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _authenticatedUser.Id ?? Guid.Empty;


            _logger.LogWarning("Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@UserName}",
                requestName, elapsedMilliseconds, userId, request);
        }

        return response;
    }
}

