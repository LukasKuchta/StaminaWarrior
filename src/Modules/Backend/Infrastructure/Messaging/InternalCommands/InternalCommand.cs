namespace Backend.Infrastructure.Messaging.InternalCommands;
internal sealed class InternalCommand
{
    private InternalCommand()
    {

    }

    private InternalCommand(
        Guid id,
        Guid internalCommandId,
        string type,
        string jsonContent,
        DateTime scheduledDate)
    {
        Id = id;
        Type = type;
        JsonContent = jsonContent;
        ScheduledDate = scheduledDate;
        InternalCommandId = internalCommandId;
    }

    public Guid Id { get; private set; }

    public Guid InternalCommandId { get; private set; }

    public string Type { get; private set; }

    public string JsonContent { get; private set; }

    public DateTime ScheduledDate { get; private set; }

    public DateTime? ProcessedDate { get; private set; }

    public void MarkAsProcessed(DateTime processedDate)
    {
        ProcessedDate = processedDate;
    }

    public static InternalCommand Create(
        Guid id,
        Guid internalCommandId,
        string type,
        string jsonContent,
        DateTime scheduledDate)
    {
        return new InternalCommand(id, internalCommandId, type, jsonContent, scheduledDate);
    }
}
