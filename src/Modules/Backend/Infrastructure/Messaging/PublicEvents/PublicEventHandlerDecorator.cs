using Backend.Application.Abstractions.Clock;
using Backend.Infrastructure.Exceptions;
using BuildingBlocks.Application.PublicEvents;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using RedLockNet;

namespace Backend.Infrastructure.Messaging.PublicEvents;

internal class PublicEventHandlerDecorator<T> : INotificationHandler<T>
    where T : IPublicEvent
{
    private readonly IDistributedLockFactory _distributedLockFactory;
    private readonly IDistributedCache _distributedCache;
    private readonly INotificationHandler<T> _decorated;
    private readonly BackendApplicationDbContext _context;
    private readonly IDateTimeProvider _dateTimeProvider;

    public PublicEventHandlerDecorator(
        IDistributedLockFactory distributedLockFactory,
        IDistributedCache distributedCache,
        INotificationHandler<T> decorated,
        BackendApplicationDbContext context,
        IDateTimeProvider dateTimeProvider)
    {
        _distributedLockFactory = distributedLockFactory;
        _distributedCache = distributedCache;
        _decorated = decorated;
        _context = context;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task Handle(T notification, CancellationToken cancellationToken)
    {
        string handlerName = _decorated.GetType().Name;

        string cacheKey = $"{notification.Id}-{handlerName}";

        string? hit;

        // expiryTime argument
        // How long the lock should be held for.
        // RedLocks will automatically extend if the process that created the RedLock is still alive and the RedLock hasn't been disposed.
        using (var redLock = await _distributedLockFactory
                                    .CreateLockAsync(cacheKey, TimeSpan.FromSeconds(3))
                                    .ConfigureAwait(false))
        {

            if (redLock.IsAcquired)
            {

                hit = await _distributedCache
                                    .GetStringAsync(cacheKey, cancellationToken)
                                    .ConfigureAwait(false);

                if (hit is not null)
                {
                    throw new PublicEventInProgressException();
                }

                await _distributedCache
                    .SetStringAsync(cacheKey, string.Empty, cancellationToken)
                    .ConfigureAwait(false);
            }
            else
            {
                throw new RedisLockNotAcquiredExpcetion();
            }
        }

        bool wasHandled = _context.Set<IdempontcyPublicEvent>()
            .Any(handled =>
                    handled.PublicEventId == notification.Id
                    && handled.HandlerName == handlerName);

        if (wasHandled)
        {
            return;
        }

        try
        {

            await _decorated.Handle(notification, cancellationToken).ConfigureAwait(false);

            _context.Set<IdempontcyPublicEvent>()
                        .Add(new IdempontcyPublicEvent(
                            notification.Id,
                            handlerName,
                            _dateTimeProvider.UtcNow()));

            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            await _distributedCache.RemoveAsync(cacheKey, cancellationToken).ConfigureAwait(false);

            throw new UnhendledPublicEventException(
                notification.GetType().Name,
                _decorated.GetType().Name,
                ex.Message);
        }
    }
}
