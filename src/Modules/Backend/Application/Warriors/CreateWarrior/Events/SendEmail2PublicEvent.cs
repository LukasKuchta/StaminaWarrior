using Backend.Domain.Warriors.Events;
using BuildingBlocks.Application.PublicEvents;

namespace Backend.Application.Warriors.CreateWarrior.Events;

public record SendEmail2PublicEvent : PublicEventBase<WarriorCreatedDomainEvent>
{
    public SendEmail2PublicEvent(WarriorCreatedDomainEvent domainEvent)
        : base(domainEvent)
    {
    }
}
