using System.Data;
using System.Runtime.Serialization;
using Backend.Application;
using Backend.Application.Abstractions.Clock;
using Backend.Infrastructure.Exceptions;
using Backend.Infrastructure.Serialization;
using BuildingBlocks.Application.PublicEvents;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Backend.Infrastructure.Messaging.Outbox;
internal sealed class ProcessOutboxCommandHandler : IRequestHandler<ProcessOutboxCommand>
{
    private readonly BackendApplicationDbContext _context;
    private readonly ISerializer _serializer;
    private readonly IPublicEventPublisher _publicEventPublisher;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ProcessOutboxMessagesOptions _outboxOptions;

    public ProcessOutboxCommandHandler(
        BackendApplicationDbContext context,
        ISerializer serializer,
        IPublicEventPublisher publicDomainEventPublisher,
        IOptions<ProcessOutboxMessagesOptions> options,
        IDateTimeProvider dateTimeProvider)
    {
        _context = context;
        _serializer = serializer;
        _publicEventPublisher = publicDomainEventPublisher;
        _dateTimeProvider = dateTimeProvider;
        _outboxOptions = options.Value;
    }

    public async Task Handle(ProcessOutboxCommand request, CancellationToken cancellationToken)
    {
        IReadOnlyList<OutboxMessage> outboxMessages = await GetOutboxMessagesAsync(cancellationToken).ConfigureAwait(false);

        foreach (OutboxMessage outboxMessage in outboxMessages)
        {
            Type? type = BackendApplicationAssembly.Instance.GetType(outboxMessage.Type);
            if (type is null)
            {
                continue; // TODO add loging
            }

            IPublicEvent? publicEvent = _serializer.Deserialize(outboxMessage.Content, type) as IPublicEvent;
            if (publicEvent is null)
            {
                continue; // TODO add loging
            }
            try
            {
                outboxMessage.IncreaseAttempt(_outboxOptions.RetryCount, _dateTimeProvider.UtcNow());
                await _publicEventPublisher.Publish(publicEvent, cancellationToken).ConfigureAwait(false);
                outboxMessage.ProcessedOnUtc = _dateTimeProvider.UtcNow();
            }
            catch (Exception ex)
            {
                if (ex is AggregateException agg)
                {
                    foreach (Exception inner in agg.InnerExceptions)
                    {
                        if (inner is UnhendledPublicEventException unhandled)
                        {
                            outboxMessage.AddError(
                                outboxMessage.Id,
                                publicEvent.GetType().Name,
                                unhandled.HandlerName,
                                unhandled.Error);
                        }
                        else
                        {
                            outboxMessage.AddError(
                               outboxMessage.Id,
                               publicEvent.GetType().Name,
                               "Unrecognized handler",
                               inner.Message);
                        }
                    }
                }
                else
                {
                    outboxMessage.AddError(
                       outboxMessage.Id,
                       publicEvent.GetType().Name,
                       "Unrecognized handler",
                       ex.Message);
                }
            }
        }
    }

    private async Task<IReadOnlyList<OutboxMessage>> GetOutboxMessagesAsync(CancellationToken cancellationToken)
    {
        // use Dapper instead
        return await _context.Set<OutboxMessage>()
            .Include(e => e.Errors)
            .Where(e => e.ProcessedOnUtc == null)
            .Take(_outboxOptions.BatchSize)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}
