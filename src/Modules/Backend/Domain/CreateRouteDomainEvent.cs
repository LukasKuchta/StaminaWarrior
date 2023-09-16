using BuildingBlocks.Domain.DomainEvents;

namespace Backend.Domain;
public class CreateRouteDomainEvent : IDomainEvent
{
    public CreateRouteDomainEvent(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}
