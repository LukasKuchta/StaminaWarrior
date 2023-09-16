using Backend.Domain.Warriors.Events;
using BuildingBlocks.Application.PublicEvents;

namespace Backend.Application.Warriors.CreateWarrior.Events;
public record SendEmail1PublicEvent : PublicEventBase<WarriorCreatedDomainEvent>
{
    public SendEmail1PublicEvent(WarriorCreatedDomainEvent domainEvent)
        : base(domainEvent)
    {
    }
}
