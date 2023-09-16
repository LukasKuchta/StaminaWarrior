namespace Backend.Application.Contracts;

public abstract record CommandBase() : ICommand, ICommandBase;

public abstract record CommandBase<TResult>() : ICommand<TResult>, ICommandBase;

