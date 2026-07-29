using Microsoft.EntityFrameworkCore;
using Otex.BuildingBlocks.Application.Messaging;
using Otex.BuildingBlocks.Domain.Events;
using Otex.BuildingBlocks.Infrastructure.Outbox.Models;

namespace Otex.BuildingBlocks.Infrastructure.Outbox;

public sealed class IdempotentDomainEventHandler<TDomainEvent, TDbContext>(
    IDomainEventHandler<TDomainEvent> decorated,
    TDbContext dbContext)
    : DomainEventHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
    where TDbContext : DbContext
{
    public override async Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var outboxMessageConsumer = new OutboxMessageConsumer(domainEvent.Id, decorated.GetType().Name);

        if (await OutboxConsumerExistsAsync(outboxMessageConsumer, cancellationToken))
        {
            return;
        }

        await decorated.Handle(domainEvent, cancellationToken);

        await InsertOutboxConsumerAsync(outboxMessageConsumer, cancellationToken);
    }
    private async Task<bool> OutboxConsumerExistsAsync(
        OutboxMessageConsumer outboxMessageConsumer,
        CancellationToken cancellationToken)
    {
        return await dbContext.Set<OutboxMessageConsumer>()
            .AnyAsync(x =>
                    x.OutboxMessageId == outboxMessageConsumer.OutboxMessageId &&
                    x.Name == outboxMessageConsumer.Name,
                cancellationToken);
    }

    private async Task InsertOutboxConsumerAsync(
        OutboxMessageConsumer outboxMessageConsumer,
        CancellationToken cancellationToken)
    {
        await dbContext.Set<OutboxMessageConsumer>().AddAsync(outboxMessageConsumer, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        // ✅ راه‌حل حل باگ JSON EF Core: 
        // پاک کردن Change Tracker باعث می‌شود هندلر بعدی که در همین Scope اجرا می‌شود،
        // آبجکت را دوباره از دیتابیس بخواند و تداخلی در آپدیت ستون‌های JSON پیش نیاید.
        dbContext.ChangeTracker.Clear();
    }
}
