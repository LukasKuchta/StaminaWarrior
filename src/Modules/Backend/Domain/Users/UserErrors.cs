using BuildingBlocks.Domain;

namespace Backend.Domain.Users;

public static class UserErrors
{
    public static readonly Error NotFound = new Error(
        "Users.NotFound",
        "User by giving id is not found.");
}
