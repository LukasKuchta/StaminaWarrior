using Backend.Application.Abstractions.Clock;
using Backend.Infrastructure.Messaging.Outbox;
using Backend.Infrastructure.Serialization;
using BuildingBlocks.Application.PublicEvents;
using BuildingBlocks.Domain.DomainEvents;
using BuildingBlocks.Domain.Entities;
using BuildingBlocks.Infrastructure.DomainEventsDispatching;
using MediatR;
using Newtonsoft.Json;

namespace Backend.Infrastructure.Messaging.DomainEvents;
internal sealed class DomainEventsDispatcher : IDomainEventsDispatcher
{
    private readonly BackendApplicationDbContext _applicationDbContext;
    private readonly IPublisher _publisher;
    private readonly IPublicEventRegister _publicDomainEventRegister;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ISerializer _serializer;

    public DomainEventsDispatcher(
        BackendApplicationDbContext applicationDbContext,
        IPublisher publisher,
        IPublicEventRegister publicDomainEventRegister,
        IDateTimeProvider dateTimeProvider,
        ISerializer serializer)
    {
        _applicationDbContext = applicationDbContext;
        _publisher = publisher;
        _publicDomainEventRegister = publicDomainEventRegister;
        _dateTimeProvider = dateTimeProvider;
        _serializer = serializer;
    }

    public async Task DispatchAsync(CancellationToken cancellationToken)
    {
        IList<IDomainEvent> domainEvents = _applicationDbContext.ChangeTracker
       .Entries<IEntity>()
       .Select(e => e.Entity)
       .SelectMany(e =>
       {
           IEnumerable<IDomainEvent> events = e.GetDomainEvents();
           e.ClearDomainEvents();
           return events;
       })
       .ToList();

        if (!domainEvents.Any())
        {
            return;
        }

        // publish consequences inside the same transaction as initiator
        IEnumerable<Task> tasks = domainEvents
           .Select(async (domainEvent) =>
           {
               await _publisher.Publish(domainEvent).ConfigureAwait(false);
           });

        await Task.WhenAll(tasks).ConfigureAwait(false);

        List<IPublicEvent<IDomainEvent>> publicEvents = new();

        foreach (IDomainEvent domainEvent in domainEvents)
        {
            string domainEventTypeName = domainEvent.GetType().Name;
            IEnumerable<Type>? publicEventTypes = _publicDomainEventRegister.TryGet(domainEventTypeName);

            if (publicEventTypes is not null)
            {
                foreach (Type publicEventType in publicEventTypes)
                {
                    IPublicEvent<IDomainEvent>? publicEvent = Activator.CreateInstance(publicEventType, new object[] { domainEvent }) as IPublicEvent<IDomainEvent>;
                    if (publicEvent is not null)
                    {
                        publicEvents.Add(publicEvent);
                    }
                }
            }
        }

        IEnumerable<OutboxMessage> outboxMessages = publicEvents
            .Select(publicEvent => new OutboxMessage(
                  Guid.NewGuid(),
                  _dateTimeProvider.UtcNow(),
                  publicEvent.GetType().FullName ?? "NULL",
                  _serializer.Serialize(publicEvent)))
            .ToList();

        // only save public events inside the same transaction as initiator
        // send it outside the boundaries
        _applicationDbContext.Set<OutboxMessage>().AddRange(outboxMessages);
    }
}
