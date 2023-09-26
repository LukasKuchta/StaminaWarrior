using Backend.Application.Contracts;

namespace Backend.Application.Abstractions.Commands;
public abstract record InternalCommandBase(Guid Id) : ICommand, IInternalCommand, ICommandBase
{
}

public abstract record InternalCommandBase<TResult>(Guid Id) : ICommand<TResult>, IInternalCommand<TResult>, ICommandBase
{
}

public interface IInternalCommand
{
    public Guid Id { get; }
}

public interface IInternalCommand<out TResult> : IInternalCommand
{
}
