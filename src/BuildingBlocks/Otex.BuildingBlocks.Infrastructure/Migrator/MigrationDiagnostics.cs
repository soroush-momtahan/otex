using System.Diagnostics;

namespace Otex.BuildingBlocks.Infrastructure.Migrator;

internal static class MigrationDiagnostics
{
    public const string ActivitySourceName = "Otex.Services.Catalog.Migrations";
    
    public static readonly ActivitySource ActivitySource = new(ActivitySourceName);
}
