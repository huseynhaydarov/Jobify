namespace Jobify.Domain.Events;

public class EmployerCreatedEvent : INotification
{
    public User User { get; }

    public EmployerCreatedEvent(User user)
    {
        User = user;
    }
}
