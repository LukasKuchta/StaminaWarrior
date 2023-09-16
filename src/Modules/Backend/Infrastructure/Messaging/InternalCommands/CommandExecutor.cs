using Backend.Application.Contracts;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Infrastructure.Messaging.InternalCommands;
internal sealed class CommandExecutor : ICommandExecutor
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public CommandExecutor(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task ExecuteCommandAsync(ICommand command)
    {
        using (IServiceScope scope = _serviceScopeFactory.CreateScope())
        {
            IMediator mediatro = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediatro.Send(command).ConfigureAwait(false);
        }
    }

    public async Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command)
    {
        using (IServiceScope scope = _serviceScopeFactory.CreateScope())
        {
            IMediator mediatro = scope.ServiceProvider.GetRequiredService<IMediator>();
            var result = await mediatro.Send<TResult>(command).ConfigureAwait(false);
            return result;
        }
    }
}
