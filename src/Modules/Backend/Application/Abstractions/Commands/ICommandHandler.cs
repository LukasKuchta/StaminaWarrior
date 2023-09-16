using Backend.Application.Contracts;
using MediatR;

namespace Backend.Application.Abstractions.Commands;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand>
where TCommand : ICommand
{
}

public interface ICommandHandler<TCommand, TResopnse> : IRequestHandler<TCommand, TResopnse>
where TCommand : ICommand<TResopnse>
{
}
