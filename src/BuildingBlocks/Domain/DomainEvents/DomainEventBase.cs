namespace BuildingBlocks.Domain.DomainEvents;

public abstract record DomainEventBase(Guid Id) : IDomainEvent;
