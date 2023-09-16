namespace Backend.Domain.WarriorPaths;

public record WarriorPathId(Guid Value)
{
    public static WarriorPathId New() => new(Guid.NewGuid());
}