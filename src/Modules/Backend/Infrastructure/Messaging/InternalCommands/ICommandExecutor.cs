using Backend.Application.Contracts;

namespace Backend.Infrastructure.Messaging.InternalCommands;
public interface ICommandExecutor
{
    Task ExecuteCommandAsync(ICommand command);

    Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command);
}
