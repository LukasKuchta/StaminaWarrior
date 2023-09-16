using BuildingBlocks.Application.PublicEvents;

namespace Backend.Infrastructure.Messaging.PublicEvents;
internal sealed class PublicEventRegister : IPublicEventRegister
{
    private readonly IDictionary<string, IList<Type>> _publicEvents;

    public PublicEventRegister(IDictionary<string, IList<Type>> publicEvents)
    {
        _publicEvents = publicEvents;
    }

    public IEnumerable<Type>? TryGet(string domainEventName)
    {
        _publicEvents.TryGetValue(domainEventName, out IList<Type>? type);

        return type;
    }
}
