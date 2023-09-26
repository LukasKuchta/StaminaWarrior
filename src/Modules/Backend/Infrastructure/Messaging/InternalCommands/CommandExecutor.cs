using Backend.Application.Contracts;
using Backend.Application.Warriors.CreateWarrior;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Infrastructure.Messaging.InternalCommands;
internal static class CommandExecutor
{
    public static async Task ExecuteCommandAsync(ICommand command)
    {
        using (IServiceScope scope = BackendCompositionRoot.CreateScope())
        {
            IMediator mediatro = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediatro.Send(command).ConfigureAwait(false);
        }
    }

    public static async Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command)
    {
        using (IServiceScope scope = BackendCompositionRoot.CreateScope())
        {
            IMediator mediatro = scope.ServiceProvider.GetRequiredService<IMediator>();            
            try
            {

            var result = await mediatro.Send<TResult>(command).ConfigureAwait(false);
                return result;
            } catch (Exception ex)
            {

            }
     

        }
        return default(TResult);
    }
}
