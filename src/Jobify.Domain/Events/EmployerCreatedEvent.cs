namespace Jobify.Domain.Events;

public class EmployerCreatedEvent : INotification
{
    public EmployerCreatedEvent(User user) => User = user;
    public User User { get; }
}
