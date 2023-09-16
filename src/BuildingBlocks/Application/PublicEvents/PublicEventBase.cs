using BuildingBlocks.Domain.DomainEvents;

namespace BuildingBlocks.Application.PublicEvents;
public abstract record PublicEventBase<TEventType> : PublicEventBase, IPublicEvent<TEventType>
    where TEventType : IDomainEvent
{
    public TEventType DomainEvent { get; }

    protected PublicEventBase(TEventType domainEvent)
        : base()
    {
        DomainEvent = domainEvent;
    }
}

public abstract record PublicEventBase : IPublicEvent
{
    public Guid Id { get; }

    protected PublicEventBase()
    {
        Id = Guid.NewGuid();
    }
}
