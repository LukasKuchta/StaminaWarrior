using Backend.Application.Contracts;
using Backend.Application.Exceptions;
using FluentValidation;
using MediatR;

namespace Backend.Application.Behaviour;

internal sealed class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommandBase
{

    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next().ConfigureAwait(false);
        }

        var validationContext = new ValidationContext<TRequest>(request);

        IList<ValidationError> validationErrors = _validators
            .Select(validator => validator.Validate(validationContext))
            .Where(validationResult => validationResult.Errors.Any())
            .SelectMany(validationResult => validationResult.Errors)
            .Select(failure => new ValidationError(failure.PropertyName, failure.ErrorMessage))
            .ToList();

        if (validationErrors.Any())
        {
            throw new Exceptions.ValidationException(validationErrors);
        }

        return await next().ConfigureAwait(false);
    }
}
