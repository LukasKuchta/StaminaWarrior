using Backend.Application.Abstractions.Commands;
using Backend.Application.Contracts;

namespace Backend.Application.Warriors.CreateWarrior;

public sealed record CreateWarriorCommand(Guid UserId, string WarriorName) : CommandBase<Guid>()
{

}

