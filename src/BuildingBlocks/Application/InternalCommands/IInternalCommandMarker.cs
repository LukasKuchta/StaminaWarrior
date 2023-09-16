namespace BuildingBlocks.Application.InternalCommands;
public interface IInternalCommandMarker
{
    Task MarkAsHandled(Guid id);
}
