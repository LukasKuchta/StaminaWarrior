using Backend.Domain.Users;
using Backend.Domain.Warriors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infrastructure.Configurations;

internal sealed class WarriorConfiguration : IEntityTypeConfiguration<Warrior>
{
    public void Configure(EntityTypeBuilder<Warrior> builder)
    {
        builder.ToTable("warriors");

        builder.HasKey(x => x.Id);

        builder.Property(w => w.Id)
           .HasConversion(
           id => id.Value,
           value => new WarriorId(value));

        builder.Property(w => w.UserId)
           .HasConversion(
           userId => userId.Value,
           value => new UserId(value));

        builder.Property(w => w.CurrentLevel)
            .HasConversion(
            level => level.Value,
            value => Level.Create(value).Value);

        builder.Property(w => w.Experience)
            .HasConversion(
            exp => exp.Value,
            value => new Experience(value));

        builder.Property(w => w.Name)
            .HasConversion(
            name => name.Value,
            value => new WarriorName(value));

        builder.HasOne<User>().WithMany().HasForeignKey(w => w.UserId);
    }
}
