using Otex.BuildingBlocks.Infrastructure.Migrator;
using Otex.Micros.Applicants.Infrastructure.Data;

namespace Otex.Micros.Applicants.Migrator;

internal sealed class Migrator(
    DatabaseMigratorService<ApplicantsDbContext> dbMigrator,
    IHostApplicationLifetime hostApplicationLifetime,
    ILogger<Microsoft.EntityFrameworkCore.Migrations.Internal.Migrator> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Starting migration worker...");
        
        try
        {
            logger.LogInformation("Step 1/2: Migrating Write Database...");
            await dbMigrator.MigrateAsync(stoppingToken);

            // logger.LogInformation("Step 2/2: Migrating Read Database...");
            // await dbMigrator.MigrateAsync(stoppingToken);
            
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
