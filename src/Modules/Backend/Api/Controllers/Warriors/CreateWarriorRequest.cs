namespace Backend.Api.Controllers.Warriors;

public sealed record CreateWarriorRequest(Guid UserId, string WarriorName);
