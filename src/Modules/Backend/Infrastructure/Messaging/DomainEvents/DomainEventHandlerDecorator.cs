using BuildingBlocks.Domain.DomainEvents;
using BuildingBlocks.Infrastructure.DomainEventsDispatching;
using MediatR;

namespace Backend.Infrastructure.Messaging.DomainEvents;

// BUG https://github.com/jbogard/MediatR/issues/718
internal class DomainEventHandlerDecorator<T> : INotificationHandler<T>
    where T : IDomainEvent
{
    private readonly IDomainEventsDispatcher _dispatcher;
    private readonly INotificationHandler<T> _decorated;

    public DomainEventHandlerDecorator(
        IDomainEventsDispatcher dispatcher,
        INotificationHandler<T> decorated)
    {
        _dispatcher = dispatcher;
        _decorated = decorated;
    }

    public async Task Handle(T notification, CancellationToken cancellationToken)
    {
        await _decorated.Handle(notification, cancellationToken).ConfigureAwait(false);
        await _dispatcher.DispatchAsync(cancellationToken).ConfigureAwait(false);
    }
}

