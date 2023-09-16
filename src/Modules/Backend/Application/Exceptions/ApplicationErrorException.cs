namespace Backend.Application.Exceptions;
public sealed class ApplicationErrorException : Exception
{
    public ApplicationErrorException(string errorCode, string errorMessage)
        : base(errorMessage)
    {
        Code = errorCode;
    }

    public string Code { get; }
}
