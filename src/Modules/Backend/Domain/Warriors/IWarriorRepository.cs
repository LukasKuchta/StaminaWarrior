namespace Backend.Domain.Warriors;

public interface IWarriorRepository
{
    Task<Warrior?> GetByIdAsync(WarriorId warriorId, CancellationToken cancellationToken = default);

    void Add(Warrior warrior);
}
