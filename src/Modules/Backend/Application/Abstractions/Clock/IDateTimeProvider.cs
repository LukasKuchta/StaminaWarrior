namespace Backend.Application.Abstractions.Clock;

public interface IDateTimeProvider
{
    DateTime UtcNow();
}
