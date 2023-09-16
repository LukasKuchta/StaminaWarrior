namespace Backend.Domain.WarriorPathPoints;

public record Speed(double? Value, SpeedUnit SpeedUnit)
{
    public static Speed Unknown() => new(null, SpeedUnit.Unknown);
    public bool IsUnknown() => !Value.HasValue;
}
