using Backend.Application.Abstractions.Clock;
using Backend.Application.Abstractions.Commands;
using Backend.Infrastructure.Exceptions;
using Backend.Infrastructure.Serialization;

namespace Backend.Infrastructure.Messaging.InternalCommands;
internal static class Extension
{
    public static InternalCommand ToInternalCommand(this IInternalCommand internalCommand, ISerializer serializer, IDateTimeProvider dateTimeProvider)
    {
        if (internalCommand is null)
        {
            throw new ArgumentNullException(nameof(internalCommand));
        }

        string? typeFullName = internalCommand.GetType().FullName;
        if (typeFullName is null)
        {
            throw new UnrecoginzedTypeFullNameException();
        }

        return InternalCommand.Create(
            Guid.NewGuid(),
            internalCommand.Id,
            typeFullName,
            serializer.Serialize(internalCommand),
            dateTimeProvider.UtcNow());
    }

    public static InternalCommand ToInternalCommand<TResult>(this IInternalCommand<TResult> internalCommand, ISerializer serializer, IDateTimeProvider dateTimeProvider)
    {
        if (internalCommand is null)
        {
            throw new ArgumentNullException(nameof(internalCommand));
        }

        string? typeFullName = internalCommand.GetType().FullName;
        if (typeFullName is null)
        {
            throw new UnrecoginzedTypeFullNameException();
        }

        string serialization = serializer.Serialize(internalCommand);

        return InternalCommand.Create(
            Guid.NewGuid(),
            internalCommand.Id,
            typeFullName,
            serialization,
            dateTimeProvider.UtcNow());
    }
}
