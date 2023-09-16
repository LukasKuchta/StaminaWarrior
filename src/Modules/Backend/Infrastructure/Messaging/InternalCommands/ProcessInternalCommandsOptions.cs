namespace Backend.Infrastructure.Messaging.InternalCommands;
internal sealed class ProcessInternalCommandsOptions
{
    public int JobProcessingInterval { get; init; }

    public int BatchSize { get; init; }
}
