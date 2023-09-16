namespace Backend.Application.WarriorPaths.GetWarriorPaths;

public sealed record WarriorPathResponse(
    Guid WarriorId,
    Guid WarriorPathId)
{
    public IList<Waypoint> Waypoints { get; } = new List<Waypoint>();
}