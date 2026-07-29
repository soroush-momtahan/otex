using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Otex.BuildingBlocks.Infrastructure.Migrator;

namespace Otex.BuildingBlocks.Infrastructure.Extensions;

public static class MigratorExtension
{
    public static IHostApplicationBuilder AddMigratorConfigurations(
        this IHostApplicationBuilder builder,
        Assembly assemblyToScanDbContexts)
    {
        builder.AddServiceDefaults();

        // Find all DbContext types inside the assembly
        var dbContextTypes = assemblyToScanDbContexts
            .GetTypes()
            .Where(t => !t.IsAbstract &&
                        typeof(DbContext).IsAssignableFrom(t))
            .ToList();

        foreach (Type dbContextType in dbContextTypes)
        {
            // Register DatabaseMigratorService<TContext>
            Type migratorServiceType = typeof(DatabaseMigratorService<>).MakeGenericType(dbContextType);
            builder.Services.AddTransient(migratorServiceType);
        }

        return builder;
    }
}
