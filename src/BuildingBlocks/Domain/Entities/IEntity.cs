using BuildingBlocks.Domain.DomainEvents;

namespace BuildingBlocks.Domain.Entities;

public interface IEntity
{
    IReadOnlyList<IDomainEvent> GetDomainEvents();

    void ClearDomainEvents();
}
