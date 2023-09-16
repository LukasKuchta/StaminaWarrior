using BuildingBlocks.Domain.DomainEvents;

namespace BuildingBlocks.Domain.Entities;

public abstract class EntityBase<TEntityId> : IEntity
{
    private readonly List<IDomainEvent> _domainEvents = new();

    protected EntityBase(TEntityId entityId)
    {
        Id = entityId;
    }

    protected EntityBase()
    {
        // EFC tools needs
    }

    public TEntityId Id { get; private set; }

    public IReadOnlyList<IDomainEvent> GetDomainEvents()
    {
        return _domainEvents.ToList();
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
