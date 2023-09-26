using Backend.Application.Exceptions;
using BuildingBlocks.Domain;
using BuildingBlocks.Infrastructure.DomainEventsDispatching;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure;
internal class BackendUnitOfWork : IUnitOfWork
{
    private readonly BackendApplicationDbContext _applicationDbContext;
    private readonly IDomainEventsDispatcher _domainEventsDispatcher;

    public BackendUnitOfWork(
        BackendApplicationDbContext applicationDbContext,
        IDomainEventsDispatcher domainEventsDispatcher)
    {
        _applicationDbContext = applicationDbContext;
        _domainEventsDispatcher = domainEventsDispatcher;        
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _domainEventsDispatcher.DispatchAsync(cancellationToken).ConfigureAwait(false);

            return await _applicationDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            // Dont leak EFC exception into application layer 
            throw new ConcurrencyException("Concurrency update!");
        }
    }
}
