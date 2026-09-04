using Otex.BuildingBlocks.Infrastructure.Extensions;
using Otex.Micros.Identity.Infrastructure;
using Otex.Micros.Identity.Infrastructure.Data;
using Otex.Micros.Identity.Migrator;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Migrator>();

builder.AddMigratorConfigurations(
    assemblyToScanDbContexts: typeof(IdentityMicro).Assembly);

builder.AddNpgsqlDbContext<IdentityUserDbContext>("identity-db");
builder.AddNpgsqlDbContext<AspIdentityDbContext>("identity-db");

var host = builder.Build();
host.Run();