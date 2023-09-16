namespace Backend.Infrastructure.Messaging.PublicEvents;
internal sealed class IdempontcyPublicEvent
{
    public IdempontcyPublicEvent(
        Guid publicEventId,
        string handlerName,
        DateTime processedDateUtc)
    {
        Id = Guid.NewGuid();
        PublicEventId = publicEventId;
        HandlerName = handlerName;
        ProcessedDateUtc = processedDateUtc;
    }

    public Guid Id { get; }

    public Guid PublicEventId { get; private set; }

    public string HandlerName { get; private set; }

    public DateTime ProcessedDateUtc { get; private set; }
}
