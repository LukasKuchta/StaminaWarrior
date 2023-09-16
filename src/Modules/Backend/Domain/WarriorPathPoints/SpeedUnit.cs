using BuildingBlocks.Domain;

namespace Backend.Domain.WarriorPathPoints;

public sealed record SpeedUnit
{
    internal static readonly SpeedUnit Unknown = new(string.Empty);
    public static readonly SpeedUnit Km = new("Km");
    public static readonly SpeedUnit Mile = new("Mile");

    public static readonly IReadOnlyCollection<SpeedUnit> SupportedUnits = new[]
    {
        Km,
        Mile,
    };

    public static Result<SpeedUnit> FromValue(string value)
    {
        SpeedUnit? result = SupportedUnits.FirstOrDefault(v => v.Value == value);

        if (result is null)
        {
            return Result.Failure<SpeedUnit>(new Error("SpeedUnit.Unsupported", "Unsupported spped unit"));
        }

        return Result.Success(result);
    }

    public string Value { get; private set; }
    private SpeedUnit(string value) => Value = value;
}
