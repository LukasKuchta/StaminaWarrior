using Backend.Infrastructure.Messaging.InternalCommands;
using Quartz;

namespace Backend.Infrastructure.Messaging.Outbox;

[DisallowConcurrentExecution]
internal sealed class ProcessOutboxMessagesJob : IJob
{
    private readonly ICommandExecutor _commandExecutor;

    public ProcessOutboxMessagesJob(ICommandExecutor commandExecutor)
    {
        _commandExecutor = commandExecutor;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _commandExecutor.ExecuteCommandAsync(new ProcessOutboxCommand()).ConfigureAwait(false);
    }
}
