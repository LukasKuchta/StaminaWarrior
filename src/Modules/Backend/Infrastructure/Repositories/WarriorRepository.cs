using Backend.Domain.Warriors;

namespace Backend.Infrastructure.Repositories;

internal sealed class WarriorRepository : RepositoryBase<Warrior, WarriorId>, IWarriorRepository
{
    public WarriorRepository(ApplicationDbContext applicationDbContext)
        : base(applicationDbContext)
    {
    }
}
