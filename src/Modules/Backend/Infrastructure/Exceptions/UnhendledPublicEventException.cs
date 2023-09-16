namespace Backend.Infrastructure.Exceptions;
internal class UnhendledPublicEventException : Exception
{
    public UnhendledPublicEventException(
        string publicEventName,
        string handlerName,
        string error)
    {
        PublicEventName = publicEventName;
        HandlerName = handlerName;
        Error = error;
    }

    public string HandlerName { get; }

    public string PublicEventName { get; }

    public string Error { get; }
}
