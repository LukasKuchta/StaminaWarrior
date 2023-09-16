namespace Backend.Domain.WarriorPathPoints;

public record GpsCoordinateAccuracy(
 float? VerticalAccuracyMeters,
 float? SpeedAccuracyMetersPerSecond,
 float? Accuracy)
{
    public static GpsCoordinateAccuracy Unknown() => new(null, null, null);
    public bool IsUnknown() => this == Unknown();
}
