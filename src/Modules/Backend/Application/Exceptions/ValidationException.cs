namespace Backend.Application.Exceptions;

public sealed class ValidationException : Exception
{
    public IEnumerable<ValidationError> ValidationErrors { get; }

    public ValidationException(IEnumerable<ValidationError> validationErrors)
    {
        ValidationErrors = validationErrors;
    }

    public ValidationException()
    {
    }

    public ValidationException(string message) 
        : base(message)
    {
    }

    public ValidationException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
