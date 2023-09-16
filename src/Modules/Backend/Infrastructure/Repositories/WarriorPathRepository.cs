using Backend.Domain.WarriorPaths;

namespace Backend.Infrastructure.Repositories;
internal class WarriorPathRepository : RepositoryBase<WarriorPath, WarriorPathId>, IWarriorPathRepository
{
    public WarriorPathRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
    }
}
