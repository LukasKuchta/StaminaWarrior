using Quartz;

namespace Backend.Infrastructure.Messaging.InternalCommands;

[DisallowConcurrentExecution]
internal sealed class ProcessInternalCommandsJob : IJob
{
    private readonly ICommandExecutor _commandExecutor;

    public ProcessInternalCommandsJob(ICommandExecutor commandExecutor)
    {
        _commandExecutor = commandExecutor;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _commandExecutor.ExecuteCommandAsync(new ProcessInternalCommandsCommand()).ConfigureAwait(false);
    }
}

