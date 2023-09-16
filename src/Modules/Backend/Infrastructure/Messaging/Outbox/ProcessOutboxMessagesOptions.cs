namespace Backend.Infrastructure.Messaging.Outbox;
internal sealed class ProcessOutboxMessagesOptions
{
    public int JobProcessingInterval { get; init; }

    public int BatchSize { get; init; }

    public int RetryCount { get; init; }
}
