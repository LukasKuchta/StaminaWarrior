namespace Backend.Domain.WarriorPathPoints;

public record GpsCoordinate(double? Lattitude, double? Longitudal, double? Alattitude)
{
    public static GpsCoordinate Unknown() => new(null, null, null);
    public bool IsUnknown() => this == Unknown();
    public bool IsComplete() => Lattitude.HasValue && Longitudal.HasValue && Alattitude.HasValue;
}
