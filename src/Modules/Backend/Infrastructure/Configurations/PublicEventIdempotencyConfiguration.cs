using Backend.Infrastructure.Messaging.PublicEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infrastructure.Configurations;
internal sealed class PublicEventIdempotencyConfiguration : IEntityTypeConfiguration<IdempontcyPublicEvent>
{
    public void Configure(EntityTypeBuilder<IdempontcyPublicEvent> builder)
    {
        builder.ToTable("idempotency_public_events");
        builder.HasKey(e => new { e.PublicEventId, e.HandlerName });
    }
}
