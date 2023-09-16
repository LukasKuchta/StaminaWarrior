namespace Backend.Infrastructure.Messaging.Outbox;
internal sealed class OutboxMessageError
{
    public OutboxMessageError(
        Guid outboxMessageId,
        string publicEventName,
        string handlerName,
        string error)
    {
        OutboxMessageId = outboxMessageId;
        PublicEventName = publicEventName;
        HandlerName = handlerName;
        Error = error;
    }

    private OutboxMessageError() { }

    public Guid OutboxMessageId { get; private set; }

    public string PublicEventName { get; private set; }

    public string HandlerName { get; private set; }

    public string Error { get; private set; }
}
