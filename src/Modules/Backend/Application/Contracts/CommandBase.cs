using Backend.Application.Abstractions.Commands;

namespace Backend.Application.Contracts;

// marker for behaviours, log and validate only commands, ot query
public interface ICommandBase { }

public abstract record CommandBase() : ICommand, ICommandBase;

public abstract record CommandBase<TResult>() : ICommand<TResult>, ICommandBase;

