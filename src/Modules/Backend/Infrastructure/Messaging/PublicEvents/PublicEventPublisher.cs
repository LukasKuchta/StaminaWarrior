using BuildingBlocks.Application.PublicEvents;
using MediatR;

namespace Backend.Infrastructure.Messaging.PublicEvents;


internal sealed class PublicEventPublisher : Mediator, IPublicEventPublisher
{
    public PublicEventPublisher(IServiceProvider serviceProvider)
        : base(serviceProvider, new UnbreakableForeachAwaitPublisher())
    {
    }
}
