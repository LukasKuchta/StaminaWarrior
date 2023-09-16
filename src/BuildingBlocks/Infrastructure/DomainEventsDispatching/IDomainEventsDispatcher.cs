namespace BuildingBlocks.Infrastructure.DomainEventsDispatching;
public interface IDomainEventsDispatcher
{
    Task DispatchAsync(CancellationToken cancellationToken);
}
