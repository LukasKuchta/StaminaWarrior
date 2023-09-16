namespace Backend.Infrastructure.Messaging.Outbox;
internal sealed record OutboxMessage
{
    private OutboxMessage() { }

    public OutboxMessage(
        Guid id,
        DateTime occurredOnUtc,
        string type,
        string content)
    {
        Id = id;
        OccurredOnUtc = occurredOnUtc;
        Type = type;
        Content = content;
        Attempt = 0;
    }

    public Guid Id { get; private set; }

    public DateTime OccurredOnUtc { get; private set; }

    public DateTime? ProcessedOnUtc { get; set; }

    public string Type { get; private set; }

    public string Content { get; private set; }

    public int Attempt { get; private set; }

    public List<OutboxMessageError> Errors { get; private set; } = new List<OutboxMessageError>();

    public void AddError(Guid messageId, string publicEventName, string handlerName, string error)
    {
        if (Errors.Any(e => e.Error.Equals(error, StringComparison.Ordinal)))
        {
            return;
        }

        Errors.Add(new OutboxMessageError(messageId, publicEventName, handlerName, error));
    }

    public void IncreaseAttempt(int attemptLimit, DateTime dateTime)
    {
        Attempt = ++Attempt;

        if (Attempt == attemptLimit)
        {
            ProcessedOnUtc = dateTime;
        }
    }
}


