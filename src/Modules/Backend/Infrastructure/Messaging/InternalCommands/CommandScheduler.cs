using Backend.Application.Abstractions.Clock;
using Backend.Application.Abstractions.Commands;
using Backend.Infrastructure.Serialization;

namespace Backend.Infrastructure.Messaging.InternalCommands;
internal class CommandScheduler : ICommandScheduler
{
    private readonly BackendApplicationDbContext _applicationDbContext;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ISerializer _serializer;

    public CommandScheduler(
        BackendApplicationDbContext applicationDbContext,
        IDateTimeProvider dateTimeProvider,
        ISerializer serializer)
    {
        _applicationDbContext = applicationDbContext;
        _dateTimeProvider = dateTimeProvider;
        _serializer = serializer;
    }

    public void Schedule(IInternalCommand command)
    {
        InternalCommand internalCommand = command.ToInternalCommand(_serializer, _dateTimeProvider);
        _applicationDbContext.Set<InternalCommand>().Add(internalCommand);
    }

    public void Schedule<TResult>(IInternalCommand<TResult> command)
    {
        InternalCommand internalCommand = command.ToInternalCommand<TResult>(_serializer, _dateTimeProvider);
        _applicationDbContext.Set<InternalCommand>().Add(internalCommand);
    }
}
