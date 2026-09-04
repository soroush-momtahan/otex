using Otex.BuildingBlocks.Infrastructure.Migrator;
using Otex.Micros.Identity.Infrastructure.Data;

namespace Otex.Micros.Identity.Migrator;

internal sealed class Migrator(
    DatabaseMigratorService<IdentityUserDbContext> identityUserMigrator,
    DatabaseMigratorService<AspIdentityDbContext> aspIdentityMigrator,
    IHostApplicationLifetime hostApplicationLifetime,
    ILogger<Microsoft.EntityFrameworkCore.Migrations.Internal.Migrator> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Starting migration worker...");
        
        try
        {
            logger.LogInformation("Migrating Database(s)...");
            await identityUserMigrator.MigrateAsync(stoppingToken);
            await aspIdentityMigrator.MigrateAsync(stoppingToken);
            
            logger.LogInformation("All migrations completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "One of the migration jobs failed. Application will shut down with error.");
            Environment.ExitCode = 1; // کد خروج خطا برای کانتینرها (K8s/Docker)
        }
        finally
        {
            hostApplicationLifetime.StopApplication();
        }
    }
}