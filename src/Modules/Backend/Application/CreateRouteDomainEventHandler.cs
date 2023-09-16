using Backend.Domain;
using MediatR;

namespace Backend.Application;
internal sealed class CreateRouteDomainEventHandler : INotificationHandler<CreateRouteDomainEvent>
{
    public Task Handle(CreateRouteDomainEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
