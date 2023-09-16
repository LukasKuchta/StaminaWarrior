using BuildingBlocks.Domain;

namespace Backend.Domain.WarriorPaths;

public static class WarriorPathErrors
{
    public static readonly Error InccorectEndDate = new("WarriorPath.IncorrectEndDate", "End date must be greather then start.");
}
