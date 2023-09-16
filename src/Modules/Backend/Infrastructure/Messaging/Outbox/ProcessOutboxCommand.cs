using Backend.Application.Contracts;

namespace Backend.Infrastructure.Messaging.Outbox;
internal sealed record ProcessOutboxCommand() : CommandBase();

