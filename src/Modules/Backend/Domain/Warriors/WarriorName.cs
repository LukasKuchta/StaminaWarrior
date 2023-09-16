namespace Backend.Domain.Warriors;

public record WarriorName(string Value)
{
    public static readonly int MinLength = 2;

    public bool IskLengthAndNullabilityValid()
    {
        return !(string.IsNullOrEmpty(Value) || Value.Length < MinLength);
    }
}
