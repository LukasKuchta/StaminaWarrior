using BuildingBlocks.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Backend.Infrastructure.Repositories;

internal abstract class RepositoryBase<TEntity, TEntityId>
    where TEntity : EntityBase<TEntityId>
    where TEntityId : class
{
    protected readonly ApplicationDbContext DbContext;

    protected RepositoryBase(ApplicationDbContext applicationDbContext)
    {
        DbContext = applicationDbContext;
    }

    public async Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancelationToken = default)
    {
        return await DbContext
            .Set<TEntity>()
            .FirstOrDefaultAsync(e => e.Id == id, cancelationToken).ConfigureAwait(false);
    }

    public void Add(TEntity entity)
    {
        DbContext.Set<TEntity>().Add(entity);
    }
}
