namespace BuildingBlocks.Domain;

public record Error(string Code, string Message)
{

    public static readonly Error None = new(string.Empty, string.Empty);

    public static readonly Error NullValue = new("Error.NullValue", "Null value was provided");

    public static readonly Error ConcurrencyViolation = new("Error.ConcurrencyViolation", "Another operation changed underlaying data protected by optimistick locking.");

    public static Error CreateUnknown(string message)
    {
        return new Error("UnknownCode", message);
    }
}
