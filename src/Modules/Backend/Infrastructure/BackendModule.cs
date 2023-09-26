using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.Contracts;
using Backend.Infrastructure.Messaging.InternalCommands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Infrastructure;
public class BackendModule : IBackendModule
{
    public async Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command)
    {
        return await CommandExecutor.ExecuteCommandAsync(command);
    }

    public async Task ExecuteCommandAsync(ICommand command)
    {
        await CommandExecutor.ExecuteCommandAsync(command);
    }

    public async Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query)
    {
        using (var scope = BackendCompositionRoot.CreateScope())
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            return await mediator.Send(query);
        }
    }
}
