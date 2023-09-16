using MediatR;

namespace BuildingBlocks.Domain.DomainEvents;

public interface IDomainEvent : INotification
{
    public Guid Id { get; }
}
