using Jobify.Domain.Entities;
using MediatR;

namespace Jobify.Domain.Events;

public class EmployerCreatedEvent : INotification
{
    public User User { get; }

    public EmployerCreatedEvent(User user)
    {
        User = user;
    }
}
