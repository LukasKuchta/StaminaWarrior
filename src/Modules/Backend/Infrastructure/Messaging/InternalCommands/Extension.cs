using Backend.Application.Abstractions.Clock;
using Backend.Application.Abstractions.Commands;
using Backend.Infrastructure.Clock;
using Backend.Infrastructure.Exceptions;
using Backend.Infrastructure.Serialization;

namespace Backend.Infrastructure.Messaging.InternalCommands;
internal static class Extension
{
    public static InternalCommand ToInternalCommand(this IInternalCommand internalCommand, ISerializer serializer, IDateTimeProvider dateTimeProvider)
    {
        return CreateInternalCommand(internalCommand, serializer, dateTimeProvider);
    }

    private static InternalCommand CreateInternalCommand(
        IInternalCommand command,
        ISerializer serializer,
        IDateTimeProvider dateTimeProvider)
    {
        string? typeFullName = command.GetType().FullName;
        if (typeFullName is null)
        {
            throw new UnrecoginzedTypeFullNameException();
        }

        string commandAsString = serializer.Serialize(command);

        return InternalCommand.Create(
       Guid.NewGuid(),
       command.Id,
       typeFullName,
       commandAsString,
       dateTimeProvider.UtcNow());
    }

    public static InternalCommand ToInternalCommand<TResult>(this IInternalCommand<TResult> internalCommand, ISerializer serializer, IDateTimeProvider dateTimeProvider)
    {
        return CreateInternalCommand(internalCommand, serializer, dateTimeProvider);
    }
}
