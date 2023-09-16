using BuildingBlocks.Domain.DomainEvents;

namespace Backend.Domain.WarriorPaths;

public sealed record WarriorPathFinishedDomainEvent(Guid Id, WarriorPathId WarriorPathId) : IDomainEvent;
