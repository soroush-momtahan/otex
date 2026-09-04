using Otex.BuildingBlocks.Infrastructure.Extensions;
using Otex.Micros.Applicants.Migrator;
using Otex.Micros.Applicants.Infrastructure;
using Otex.Micros.Applicants.Infrastructure.Data;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Migrator>();

builder.AddMigratorConfigurations(
    assemblyToScanDbContexts: typeof(ApplicantsMicro).Assembly);

builder.AddNpgsqlDbContext<ApplicantsDbContext>("applicants-db");

var host = builder.Build();
host.Run();