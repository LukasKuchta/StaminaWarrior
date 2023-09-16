using Backend.Domain.WarriorPaths;
using Backend.Domain.Warriors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infrastructure.Configurations;
internal sealed class WarriorPathConfiguration : IEntityTypeConfiguration<WarriorPath>
{
    public void Configure(EntityTypeBuilder<WarriorPath> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(w => w.Id)
           .HasConversion(
           id => id.Value,
           value => new WarriorPathId(value));

        builder.Property(w => w.WarriorId)
           .HasConversion(
           id => id.Value,
           value => new WarriorId(value));
    }
}
