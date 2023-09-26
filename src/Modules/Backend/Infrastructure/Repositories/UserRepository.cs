using Backend.Domain.Users;

namespace Backend.Infrastructure.Repositories;

internal sealed class UserRepository : RepositoryBase<User, UserId>, IUserRepository
{
    public UserRepository(BackendApplicationDbContext applicationDbContext) 
        : base(applicationDbContext)
    {
    }
}
