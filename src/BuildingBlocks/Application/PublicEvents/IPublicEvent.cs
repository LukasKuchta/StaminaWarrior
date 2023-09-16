using BuildingBlocks.Domain.DomainEvents;
using MediatR;

namespace BuildingBlocks.Application.PublicEvents;
public interface IPublicEvent<out TEventType> : IPublicEvent
    where TEventType : IDomainEvent
{
    TEventType DomainEvent { get; }
}

public interface IPublicEvent : INotification
{
    Guid Id { get; }
}


