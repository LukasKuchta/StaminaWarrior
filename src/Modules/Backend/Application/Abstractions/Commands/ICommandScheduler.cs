namespace Backend.Application.Abstractions.Commands;
public interface ICommandScheduler
{
    void Schedule<TResult>(IInternalCommand<TResult> command);

    void Schedule(IInternalCommand command);
}
