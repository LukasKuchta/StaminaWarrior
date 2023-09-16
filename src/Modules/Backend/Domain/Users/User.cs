using BuildingBlocks.Domain.Entities;

namespace Backend.Domain.Users;

public sealed class User : EntityBase<UserId>
{
    private const uint MaxLimitPerUser = 300000;

    private User(UserId entityId, string firstName, string lastName, string email)
        : base(entityId)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    private User()
    {
        // For EFC internal use
    }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public string Email { get; private set; }

    public int WarriorsCount { get; private set; }

    public static User Create(UserId userId, string firstName, string lastName, string email)
    {
        var user = new User(userId, firstName, lastName, email);

        return user;
    }

    public void IncreaseWarriorCount()
    {
        WarriorsCount++;
    }

    public bool IsMaxLimitPerUserReached()
    {
        return WarriorsCount >= MaxLimitPerUser;
    }
}
