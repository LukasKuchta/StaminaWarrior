using Backend.Application.Contracts;
using Backend.Application.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Backend.Application.Behaviour;

internal sealed class LoggingBehaviour<TRequest, TResopnse> : IPipelineBehavior<TRequest, TResopnse>
    where TRequest : ICommandBase
{
    private readonly ILogger<TRequest> _logger;

    public LoggingBehaviour(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResopnse> Handle(TRequest request, RequestHandlerDelegate<TResopnse> next, CancellationToken cancellationToken)
    {
        string commandName = request.GetType().Name;
        try
        {
            _logger.CommandExecuting(commandName);

            TResopnse result = await next().ConfigureAwait(false);

            using var state = _logger.AppendStateScope("Success");

            return result;
        }
        catch (Exception ex)
        {
            _logger.CommandExecutionError(ex);
            throw;
        }
    }
}
