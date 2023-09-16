namespace Backend.Domain.WarriorPathPoints;

public sealed record GpsCoordinateProvider(string? Value)
{
    public static GpsCoordinateProvider Unknown() => new(Value: null);
    public bool IsUnknown() => this == Unknown();
}
