using Backend.Application.Abstractions.Commands;
using Backend.Application.Exceptions;
using Backend.Domain.Users;
using Backend.Domain.Warriors;
using BuildingBlocks.Domain;

namespace Backend.Application.Warriors.CreateWarrior;

internal sealed class CreateWarriorCommandHandler : ICommandHandler<CreateWarriorCommand, Guid>
{
    private readonly IWarriorRepository _warriorRepository;
    private readonly IUserRepository _userRepository;

    public CreateWarriorCommandHandler(
        IWarriorRepository warriorRepository,
        IUserRepository userRepository)
    {
        _warriorRepository = warriorRepository;
        _userRepository = userRepository;
    }

    public async Task<Guid> Handle(CreateWarriorCommand request, CancellationToken cancellationToken)
    {
        User? user = await _userRepository.GetByIdAsync(
            new UserId(request.UserId),
            cancellationToken).ConfigureAwait(false);

        if (user is null)
        {
            throw new ApplicationErrorException(UserErrors.NotFound.Code, UserErrors.NotFound.Message);
        }

        Result<Warrior> creationResult = Warrior.Create(
            user,
            new WarriorName(request.WarriorName));

        if (creationResult.IsSuccess)
        {
            Warrior warrior = creationResult.Value;

            _warriorRepository.Add(warrior);

            return warrior.Id.Value;
        }

        throw new ApplicationErrorException(creationResult.Error.Code, creationResult.Error.Message);
    }
}
