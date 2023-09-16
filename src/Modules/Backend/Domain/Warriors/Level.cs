using BuildingBlocks.Domain;

namespace Backend.Domain.Warriors;

public sealed record Level
{
    public static readonly Level Zero = new(0);
    public uint Value { get; private set; }
    private Level(uint value)
    {
        Value = value;
    }

    public static Result<Level> Create(uint value)
    {
        if (value < 0)
        {
            return Result.Failure<Level>(WarriorErrors.NegativeLevel);
        }

        return Result.Success<Level>(new(value));
    }

    public static Result<Level> Up(Level current)
    {
        if (current is null)
        {
            return Result.Failure<Level>(Error.NullValue);
        }

        return Create(current.Value + 1);
    }

    public static Result<Level> Down(Level current)
    {
        if (current is null)
        {
            return Result.Failure<Level>(Error.NullValue);
        }

        return Create(current.Value - 1);
    }
}
