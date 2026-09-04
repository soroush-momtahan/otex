namespace Otex.Micros.Identity.Application.Abstraction;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    // متد جدید و حرفه‌ای برای تراکنش‌ها
    Task ExecuteTransactionAsync(Func<Task> action, CancellationToken cancellationToken = default);
}