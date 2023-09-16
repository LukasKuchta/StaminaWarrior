using Backend.Application.Contracts;

namespace Backend.Application.Abstractions.Commands;
public abstract record InternalCommandBase(Guid Id) : ICommand, IInternalCommand
{
}

public abstract record InternalCommandBase<TResult>(Guid Id) : ICommand<TResult>, IInternalCommand<TResult>
{
}

public interface IInternalCommand
{
    public Guid Id { get; }
}

public interface IInternalCommand<out TResult> : IInternalCommand
{
}
