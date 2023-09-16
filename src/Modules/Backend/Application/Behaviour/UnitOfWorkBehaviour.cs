using Backend.Application.Abstractions.Commands;
using BuildingBlocks.Application.InternalCommands;
using BuildingBlocks.Domain;
using MediatR;

namespace Backend.Application.Behaviour;
internal sealed class UnitOfWorkBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : notnull
{
    private readonly IInternalCommandMarker _internalCommandMarker;
    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkBehaviour(
        IInternalCommandMarker internalCommandMarker,
        IUnitOfWork unitOfWork)
    {
        _internalCommandMarker = internalCommandMarker;
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        TResponse result = await next().ConfigureAwait(false);

        if (request is IInternalCommand internalCommand)
        {
            await _internalCommandMarker.MarkAsHandled(internalCommand.Id).ConfigureAwait(false);
        }

        await _unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
        return result;
    }
}
