namespace Jobify.Application.Common.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IAuthenticatedUserService _authenticatedUserService;
    private readonly ILogger<TRequest> _logger;
    private readonly Stopwatch _timer;

    public PerformanceBehaviour(
        ILogger<TRequest> logger,
        IAuthenticatedUserService authenticatedUserService
    )
    {
        _timer = new Stopwatch();

        _logger = logger;
        _authenticatedUserService = authenticatedUserService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _timer.Start();

        TResponse response = await next();

        _timer.Stop();

        long elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > 500)
        {
            string requestName = typeof(TRequest).Name;
            Guid? userId = _authenticatedUserService.Id;


            _logger.LogWarning(
                "Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@UserName}",
                requestName, elapsedMilliseconds, userId, request);
        }

        return response;
    }
}
