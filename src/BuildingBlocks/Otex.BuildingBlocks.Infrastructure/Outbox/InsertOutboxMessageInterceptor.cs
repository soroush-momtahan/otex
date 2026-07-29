using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using Otex.BuildingBlocks.Domain.Objects;
using Otex.BuildingBlocks.Infrastructure.Outbox.Models;
using Otex.BuildingBlocks.Infrastructure.Serialization;

namespace Otex.BuildingBlocks.Infrastructure.Outbox;

public class InsertOutboxMessageInterceptor
    : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, 
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        if (eventData.Context is not null)
        {
            await InsertOutboxMessage(eventData.Context);
        }
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
    private static async Task InsertOutboxMessage(DbContext context)
    {
        IEnumerable<IAggregate> outboxMessages2 = context
            .ChangeTracker
            .Entries<IAggregate>()
            .Where(a => a.Entity.DomainEvents.Any())
            .Select(entry => entry.Entity);
        Console.WriteLine(outboxMessages2);
        var outboxMessages = context
            .ChangeTracker
            .Entries<IAggregate>()
            .Where(a => a.Entity.DomainEvents.Any())
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                // ✅ مرحله 1: گرفتن یک کپی قطعی از ایونت‌ها در مموری با ToList
                var domainEventsCopy = entity.DomainEvents.ToList(); 
                
                // ✅ مرحله 2: حالا با خیال راحت لیست اصلی داخل انتیتی را پاک می‌کنیم
                entity.Clear(); 
                
                // ✅ مرحله 3: کپیِ گرفته شده را برمی‌گردانیم
                return domainEventsCopy; 
            })
            .Select(domainEvent=> new OutboxMessage()
            {
                Id = domainEvent.Id,
                Type = domainEvent.GetType().Name,
                Content = JsonConvert.SerializeObject(domainEvent, SerializerSettings.Instance),
                OccurredOnUtc = domainEvent.OccuredOn
            })
            .ToList();

        if (outboxMessages.Any())
        {
            await context.Set<OutboxMessage>().AddRangeAsync(outboxMessages);
        }
    }
}
