using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Otex.BuildingBlocks.Infrastructure.Outbox.Models;

namespace Otex.BuildingBlocks.Infrastructure.Outbox.Configurations;

public class OutboxMessageConsumerConfiguration : IEntityTypeConfiguration<OutboxMessageConsumer>
{

    public void Configure(EntityTypeBuilder<OutboxMessageConsumer> builder)
    {
        builder.ToTable("OutboxMessageConsumer");
        builder.HasKey(x => new { x.Name, x.OutboxMessageId });
        builder.Property(o => o.Name).HasMaxLength(500);
    }
}
