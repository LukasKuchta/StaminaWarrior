using Backend.Application.Abstractions.Clock;
using BuildingBlocks.Application.InternalCommands;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Messaging.InternalCommands;
internal sealed class InternalCommandMarker : IInternalCommandMarker
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly BackendApplicationDbContext _applicationDbContext;

    public InternalCommandMarker(
        IDateTimeProvider dateTimeProvider,
        BackendApplicationDbContext applicationDbContext)
    {
        _dateTimeProvider = dateTimeProvider;
        _applicationDbContext = applicationDbContext;
    }

    public async Task MarkAsHandled(Guid id)
    {
        InternalCommand? command = await _applicationDbContext.Set<InternalCommand>().FirstOrDefaultAsync(c => c.InternalCommandId == id).ConfigureAwait(false);

        if (command is not null)
        {
            command.MarkAsProcessed(_dateTimeProvider.UtcNow());
        }
    }
}
