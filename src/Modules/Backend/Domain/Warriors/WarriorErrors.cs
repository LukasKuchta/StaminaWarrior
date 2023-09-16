using BuildingBlocks.Domain;

namespace Backend.Domain.Warriors;

public static class WarriorErrors
{
    public static readonly Error InvalidName = new(
"Warrior.InvalidName",
$"The warrior name is empty or lenght is lower then {WarriorName.MinLength}!");

    public static readonly Error NotFound = new(
"Warrior.NotFound",
"The warrior with the specified identifier was not found");

    public static readonly Error NegativeLevel = new(
"Warrior.NegativeLevel",
"Level down is not possible. Current level is zero.");

    public static readonly Error MaxLimitPerUserReached = new(
"Warrior.MaxLimitPerUserReached",
"Max limit per user was reached.");

    public static readonly Error NotCreated = new(
"Warrior.NotCreated",
"Ilegal input check warrior parameters.");
}
