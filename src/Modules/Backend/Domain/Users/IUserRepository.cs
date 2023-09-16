namespace Backend.Domain.Users;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(UserId userId, CancellationToken cancellationToken = default);
}
