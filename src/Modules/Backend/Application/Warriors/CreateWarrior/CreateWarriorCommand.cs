using Backend.Application.Abstractions.Commands;

namespace Backend.Application.Warriors.CreateWarrior;

public sealed record CreateWarriorCommand(Guid UserId, string WarriorName) : InternalCommandBase<Guid>(Guid.NewGuid())
{

}

