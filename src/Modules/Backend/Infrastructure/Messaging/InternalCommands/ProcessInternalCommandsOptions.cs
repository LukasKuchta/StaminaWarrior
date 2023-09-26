namespace Backend.Infrastructure.Messaging.InternalCommands;
public sealed class ProcessInternalCommandsOptions
{
    public int JobProcessingInterval { get; init; }

    public int BatchSize { get; init; }
}
