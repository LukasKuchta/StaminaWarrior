using Backend.Application.Abstractions.Commands;
using Backend.Application.Contracts;
using Backend.Domain.Users;

namespace Backend.Application.Warriors.DeleteWarrior;
public sealed record DeleteWarriorCommand() : CommandBase<int>();

internal sealed class DeleteWarriorCommandHandler : ICommandHandler<DeleteWarriorCommand, int>
{
    public DeleteWarriorCommandHandler(IUserRepository userRepository)
    {

    }

    public Task<int> Handle(DeleteWarriorCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(1);
    }


}
