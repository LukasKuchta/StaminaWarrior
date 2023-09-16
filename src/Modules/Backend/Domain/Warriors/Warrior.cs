using Backend.Domain.Users;
using Backend.Domain.Warriors.Events;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.Entities;

namespace Backend.Domain.Warriors;

public sealed class Warrior : EntityBase<WarriorId>
{
    private Warrior(
     WarriorId entityId,
     UserId userId,
     WarriorName warriorName)
        : base(entityId)
    {
        Name = warriorName;
        UserId = userId;
    }

    private Warrior()
    {
        // efc only
    }

    public UserId UserId { get; private set; }

    public WarriorName Name { get; private set; }

    public Level CurrentLevel { get; private set; } = Level.Zero;

    public Experience Experience { get; private set; } = Experience.Zero;

    public static Result<Warrior> Create(
        User user,
        WarriorName warriorName)
    {
        if (user is null)
        {
            return Result.Failure<Warrior>(Error.NullValue);
        }

        if (warriorName is null)
        {
            return Result.Failure<Warrior>(Error.NullValue);
        }

        if (!warriorName.IskLengthAndNullabilityValid())
        {
            return Result.Failure<Warrior>(WarriorErrors.InvalidName);
        }

        if (user.IsMaxLimitPerUserReached())
        {
            return Result.Failure<Warrior>(WarriorErrors.MaxLimitPerUserReached);
        }

        var warrior = new Warrior(WarriorId.New(), user.Id, warriorName);
        user.IncreaseWarriorCount();

        warrior.RaiseDomainEvent(new WarriorCreatedDomainEvent(
            Guid.NewGuid(),
            warrior.Id,
            warrior.Name));

        return warrior;
    }

    public Result LevelUp()
    {
        Result<Level> result = Level.Up(CurrentLevel);
        if (result.IsSuccess)
        {
            CurrentLevel = result.Value;
        }

        return result;
    }

    public Result LeveDown()
    {
        Result<Level> result = Level.Down(CurrentLevel);
        if (result.IsSuccess)
        {
            CurrentLevel = result.Value;
        }

        return result;
    }
}
