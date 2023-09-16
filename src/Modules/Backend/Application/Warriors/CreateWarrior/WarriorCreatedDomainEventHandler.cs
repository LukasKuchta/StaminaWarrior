using Backend.Application.Abstractions.Clock;
using Backend.Domain.WarriorPaths;
using Backend.Domain.Warriors.Events;
using MediatR;

namespace Backend.Application.Warriors.CreateWarrior;

internal sealed class WarriorCreatedDomainEventHandler : INotificationHandler<WarriorCreatedDomainEvent>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IWarriorPathRepository _warriorPathRepository;

    public WarriorCreatedDomainEventHandler(
        IDateTimeProvider dateTimeProvider,
        IWarriorPathRepository warriorPathRepository)
    {
        _dateTimeProvider = dateTimeProvider;
        _warriorPathRepository = warriorPathRepository;
    }

    public Task Handle(WarriorCreatedDomainEvent notification, CancellationToken cancellationToken)
    {

        var x = WarriorPath.Create(notification.WarriorId, _dateTimeProvider.UtcNow());
        _warriorPathRepository.Add(x);

        return Task.CompletedTask;
    }
}
