using Jobify.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Jobify.Application.UseCases.Users.EventHandlers;

public class EmployerCreatedEventHandler : INotificationHandler<EmployerCreatedEvent>
{
    private readonly ILogger<EmployerCreatedEventHandler> _logger;
    private readonly IApplicationDbContext _dbContext;

    public EmployerCreatedEventHandler(ILogger<EmployerCreatedEventHandler> logger, IApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task Handle(EmployerCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: {DomainEvent}", notification.GetType().Name);


        var employer = notification.User;

        var company = new Company
        {
            Name = "New Company",
            Description = $"Company for user {employer.Email}",
            CreatedById = employer.Id,
            WebsiteUrl = null,
            Industry = null
        };

        await _dbContext.Companies.AddAsync(company, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
