using Microsoft.EntityFrameworkCore;
using Otex.Micros.Identity.Application.Abstraction;
using Otex.Micros.Identity.Domain.Identity.Models;
using Otex.Micros.Identity.Infrastructure.Identity.Configurations;

namespace Otex.Micros.Identity.Infrastructure.Data;

public class IdentityUserDbContext(DbContextOptions<IdentityUserDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(Schemas.Identity);
        modelBuilder.ApplyConfiguration(new IdentityConfiguration());
    }

    public async Task ExecuteTransactionAsync(Func<Task> action, CancellationToken cancellationToken = default)
    {
        // استفاده از Execution Strategy برای پشتیبانی از Retry Pattern
        var strategy = Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            // شروع تراکنش واقعی دیتابیس
            await using var transaction = await Database.BeginTransactionAsync(cancellationToken);

            try
            {
                // اجرای کدهای بیزینسی تو (که از لایه اپلیکیشن پاس داده شده)
                await action();

                // کامیت کردن تراکنش
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                // رول‌بک در صورت بروز هرگونه خطا
                await transaction.RollbackAsync(cancellationToken);
                throw; // پاس دادن خطا به لایه‌های بالاتر
            }
        });
    }
}