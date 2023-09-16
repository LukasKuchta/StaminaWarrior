using Backend.Application;
using Backend.Infrastructure.Serialization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Backend.Infrastructure.Messaging.InternalCommands;
internal class ProcessInternalCommandsCommandHandler : IRequestHandler<ProcessInternalCommandsCommand>
{
    private readonly ISerializer _serializer;
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ICommandExecutor _commandExecutor;

    public ProcessInternalCommandsCommandHandler(
        ISerializer serializer,
        ApplicationDbContext applicationDbContext,
        ICommandExecutor commandExecutor)
    {
        _serializer = serializer;
        _applicationDbContext = applicationDbContext;
        _commandExecutor = commandExecutor;
    }

    public async Task Handle(ProcessInternalCommandsCommand request, CancellationToken cancellationToken)
    {
        IEnumerable<InternalCommand> internalCommands = await _applicationDbContext
                                                            .Set<InternalCommand>()
                                                            .Where(c => c.ProcessedDate == null)
                                                            .OrderBy(c => c.ScheduledDate)
                                                            .ToListAsync(cancellationToken)
                                                            .ConfigureAwait(false);

        foreach (InternalCommand internalCommand in internalCommands)
        {
            Type? type = BackendApplicationAssembly.Instance.GetType(internalCommand.Type);
            if (type is null)
            {
                // TODO add log
                continue;
            }

            dynamic? commandToProcess = _serializer.Deserialize(internalCommand.JsonContent, type);

            if (commandToProcess is null)
            {
                // TODO add log
                continue;
            }

            // TODO add retry policy 

            await _commandExecutor.ExecuteCommandAsync(commandToProcess);

            // check policy state and mark as processed with error

            // mark as processed is happen inside the command handler decorator/ UnitOfWorkBehaviour
        }
    }
}
