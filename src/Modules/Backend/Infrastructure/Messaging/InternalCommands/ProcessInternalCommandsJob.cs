using Quartz;

namespace Backend.Infrastructure.Messaging.InternalCommands;

[DisallowConcurrentExecution]
internal sealed class ProcessInternalCommandsJob : IJob
{  
    public async Task Execute(IJobExecutionContext context)
    {        
        await CommandExecutor.ExecuteCommandAsync(new ProcessInternalCommandsCommand()).ConfigureAwait(false);
    }
}

