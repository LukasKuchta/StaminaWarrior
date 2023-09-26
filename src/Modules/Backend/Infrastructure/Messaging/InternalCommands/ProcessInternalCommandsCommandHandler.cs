using Backend.Application;
using Backend.Application.Abstractions.Clock;
using Backend.Infrastructure.Serialization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using Polly;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Backend.Infrastructure.Messaging.InternalCommands;
internal class ProcessInternalCommandsCommandHandler : IRequestHandler<ProcessInternalCommandsCommand>
{
    private readonly ProcessInternalCommandsOptions _options;
    private readonly ISerializer _serializer;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly BackendApplicationDbContext _applicationDbContext;
    private readonly ICommandExecutor _commandExecutor;

    public ProcessInternalCommandsCommandHandler(
        IOptions<ProcessInternalCommandsOptions> options,
        ISerializer serializer,
        IDateTimeProvider dateTimeProvider,
        BackendApplicationDbContext applicationDbContext,
        ICommandExecutor commandExecutor)
    {
        _options = options.Value;
        _serializer = serializer;
        _dateTimeProvider = dateTimeProvider;
        _applicationDbContext = applicationDbContext;
        _commandExecutor = commandExecutor;
    }

    public async Task Handle(ProcessInternalCommandsCommand request, CancellationToken cancellationToken)
    {        
        IEnumerable<InternalCommand> internalCommands = await _applicationDbContext
                                                            .Set<InternalCommand>()
                                                            .Where(c => c.ProcessedDate == null)
                                                            .Take(_options.BatchSize)
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

            var policy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(new[]
            {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(3)
            });

            PolicyResult result = await policy.ExecuteAndCaptureAsync(() =>
            {
                return _commandExecutor.ExecuteCommandAsync(commandToProcess);
            });

            if (result.Outcome == OutcomeType.Failure)
            {
                internalCommand.MarkAsProcessedWithError(_dateTimeProvider.UtcNow(), result.FinalException.Message);
            }

            // mark as processed without error is happen inside the command handler decorator / UnitOfWorkBehaviour
        }
    }
}
