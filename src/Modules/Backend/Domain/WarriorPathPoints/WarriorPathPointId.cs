namespace Backend.Domain.WarriorPathPoints;

public record WarriorPathPointId(Guid Value)
{
    public static WarriorPathPointId New() => new(Guid.NewGuid());
}
