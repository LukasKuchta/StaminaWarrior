namespace BuildingBlocks.Application.PublicEvents;
public interface IPublicEventRegister
{
    IEnumerable<Type>? TryGet(string domainEventName);
}
