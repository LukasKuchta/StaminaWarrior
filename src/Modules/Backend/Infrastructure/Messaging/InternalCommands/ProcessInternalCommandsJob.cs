using Quartz;

namespace Backend.Infrastructure.Messaging.InternalCommands;

[DisallowConcurrentExecution]
internal sealed class ProcessInternalCommandsJob : IJob
{
  

    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("ProcessInternalCommandsJob");
        await CommandExecutor.ExecuteCommandAsync(new ProcessInternalCommandsCommand()).ConfigureAwait(false);
    }
}

