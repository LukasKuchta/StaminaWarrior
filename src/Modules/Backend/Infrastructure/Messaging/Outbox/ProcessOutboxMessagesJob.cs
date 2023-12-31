﻿using Backend.Infrastructure.Messaging.InternalCommands;
using Quartz;

namespace Backend.Infrastructure.Messaging.Outbox;

[DisallowConcurrentExecution]
internal sealed class ProcessOutboxMessagesJob : IJob
{    
    public async Task Execute(IJobExecutionContext context)
    {        
        await CommandExecutor.ExecuteCommandAsync(new ProcessOutboxCommand()).ConfigureAwait(false);
    }
}
