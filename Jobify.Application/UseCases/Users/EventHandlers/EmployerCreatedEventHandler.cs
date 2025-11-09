using Jobify.Domain.Enums;
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

        var user = notification.User;

        var company = new Company
        {
            Name = "New Company",
            Description = $"Company for employer {user.Email}",
            CreatedById = user.Id,
            WebsiteUrl = null,
            Industry = null
        };

        _dbContext.Companies.Add(company);

        var employer = new Employer
        {
            UserId = user.Id,
            CompanyId = company.Id,
            Position = EmployerPosition.CEO,
            JoinedAt = DateTimeOffset.UtcNow
        };
        _dbContext.Employers.Add(employer);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
