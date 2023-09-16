using Backend.Domain.WarriorPaths;
using BuildingBlocks.Domain.Entities;

namespace Backend.Domain.WarriorPathPoints;

public sealed class WarriorPathPoint : EntityBase<WarriorPathPointId>
{
    private WarriorPathPoint(
        WarriorPathPointId entityId,
        WarriorPathId warriorPathId,
        DateTime createdOn,
        GpsCoordinate gpsCoordinate,
        Speed speed,
        GpsCoordinateAccuracy accuracy,
        GpsCoordinateProvider provider)
        : base(entityId)
    {
        WarriorPathId = warriorPathId;
        CreatedOn = createdOn;
        GpsCoordinate = gpsCoordinate;
        Speed = speed;
        Accuracy = accuracy;
        Provider = provider;
    }

    public WarriorPathId WarriorPathId { get; private set; }

    public GpsCoordinate GpsCoordinate { get; private set; }

    public Speed Speed { get; private set; }

    public GpsCoordinateAccuracy Accuracy { get; private set; }

    public GpsCoordinateProvider Provider { get; private set; }

    public DateTime CreatedOn { get; private set; }

    public static WarriorPathPoint Create(
        WarriorPathId warriorPathId,
        GpsCoordinate gpsCoordinate,
        Speed speed,
        GpsCoordinateAccuracy accuracy,
        GpsCoordinateProvider provider,
        DateTime createdOn)
    {
        if (warriorPathId is null)
        {
            throw new ArgumentNullException(nameof(warriorPathId));
        }

        if (gpsCoordinate is null)
        {
            throw new ArgumentNullException(nameof(gpsCoordinate));
        }

        if (speed is null)
        {
            throw new ArgumentNullException(nameof(speed));
        }

        if (accuracy is null)
        {
            throw new ArgumentNullException(nameof(accuracy));
        }

        if (provider is null)
        {
            throw new ArgumentNullException(nameof(provider));
        }

        var point = new WarriorPathPoint(
            WarriorPathPointId.New(),
            warriorPathId,
            createdOn,
            gpsCoordinate,
            speed,
            accuracy,
            provider);

        return point;
    }
}
