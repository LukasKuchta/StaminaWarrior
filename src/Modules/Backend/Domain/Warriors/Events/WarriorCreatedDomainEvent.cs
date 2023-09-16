using BuildingBlocks.Domain.DomainEvents;

namespace Backend.Domain.Warriors.Events;

public record WarriorCreatedDomainEvent(Guid Id, WarriorId WarriorId, WarriorName WarriorName)
    : DomainEventBase(Id);
