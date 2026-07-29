using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Otex.BuildingBlocks.Infrastructure.Inbox.Models;

namespace Otex.BuildingBlocks.Infrastructure.Inbox.Configurations;

public sealed class InboxMessageConsumerConfiguration : IEntityTypeConfiguration<InboxMessageConsumer>  
{  
    public void Configure(EntityTypeBuilder<InboxMessageConsumer> builder) 
    {        builder.ToTable("InboxMessageConsumers");
  
        builder.HasKey(o => new { o.InboxMessageId, o.Name });
  
        builder.Property(o => o.Name).HasMaxLength(500);
    }
}
