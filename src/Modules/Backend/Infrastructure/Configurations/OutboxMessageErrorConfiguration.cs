using Backend.Infrastructure.Messaging.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infrastructure.Configurations;
internal sealed class OutboxMessageErrorConfiguration : IEntityTypeConfiguration<OutboxMessageError>
{
    public void Configure(EntityTypeBuilder<OutboxMessageError> builder)
    {
        builder.ToTable("outbox_message_errors");
        builder.HasKey(x => new { x.OutboxMessageId, x.HandlerName });
    }
}
