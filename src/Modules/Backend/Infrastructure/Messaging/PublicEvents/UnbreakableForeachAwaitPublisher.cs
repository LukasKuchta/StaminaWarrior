using MediatR;

namespace Backend.Infrastructure.Messaging.PublicEvents;

public class UnbreakableForeachAwaitPublisher : INotificationPublisher
{
    public async Task Publish(
        IEnumerable<NotificationHandlerExecutor> handlerExecutors,
        INotification notification,
        CancellationToken cancellationToken)
    {
        if (handlerExecutors is null) 
        { 
            throw new ArgumentNullException(nameof(handlerExecutors));
        }

        IList<Exception>? exceptionList = new List<Exception>();

        foreach (var handler in handlerExecutors)
        {
            try
            {
                await handler.HandlerCallback(notification, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                exceptionList.Add(ex);
            }
        }

        if (exceptionList.Any())
        {
            throw new AggregateException(
                        "Encountered errors while trying to publish public events.",
                        exceptionList);
        }
    }
}
