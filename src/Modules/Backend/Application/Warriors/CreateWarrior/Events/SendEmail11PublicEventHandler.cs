using MediatR;

namespace Backend.Application.Warriors.CreateWarrior.Events;
public class SendEmail11PublicEventHandler : INotificationHandler<SendEmail1PublicEvent>
{
    public Task Handle(SendEmail1PublicEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

public class SendEmail12PublicEventHandler : INotificationHandler<SendEmail1PublicEvent>
{
    public Task Handle(SendEmail1PublicEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

public class SendEmail13PublicEventHandler : INotificationHandler<SendEmail1PublicEvent>
{
    public Task Handle(SendEmail1PublicEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

public class SendEmail21PublicEventHandler : INotificationHandler<SendEmail2PublicEvent>
{
    public Task Handle(SendEmail2PublicEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
