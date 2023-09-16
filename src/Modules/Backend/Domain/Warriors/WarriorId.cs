namespace Backend.Domain.Warriors;

public record WarriorId(Guid Value)
{
    public static WarriorId New() => new(Guid.NewGuid());
}
