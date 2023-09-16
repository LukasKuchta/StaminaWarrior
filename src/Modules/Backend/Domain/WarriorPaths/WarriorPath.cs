using Backend.Domain.Warriors;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.Entities;

namespace Backend.Domain.WarriorPaths;

public sealed class WarriorPath : EntityBase<WarriorPathId>
{
    private WarriorPath()
    {
    }

    private WarriorPath(
        WarriorPathId entityId,
        WarriorId warriorId,
        DateTime start)
        : base(entityId)
    {
        Start = start;
        WarriorId = warriorId;
    }

    public WarriorId WarriorId { get; private set; }

    public DateTime Start { get; private set; }

    public DateTime? End { get; private set; }

    public static WarriorPath Create(
        WarriorId warriorId,
        DateTime start)
    {
        if (warriorId is null)
        {
            throw new ArgumentNullException(nameof(warriorId));
        }

        WarriorPath path = new(WarriorPathId.New(), warriorId, start);

        path.RaiseDomainEvent(new CreateRouteDomainEvent(Guid.NewGuid()));
        return path;
    }

    public Result FinishPath(DateTime end)
    {
        if (end < Start)
        {
            return Result.Failure(WarriorPathErrors.InccorectEndDate);
        }

        End = end;

        return Result.Success();
    }
}
