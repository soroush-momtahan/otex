using Otex.BuildingBlocks.Api;
using Otex.BuildingBlocks.Application;
using Otex.BuildingBlocks.Infrastructure;
using Otex.Micros.Applicants.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddApiConfiguration(assembliesToScanEndpoints: Otex.Micros.Applicants.Presentation.AssemblyReference.Presentation)
    .AddApplicationConfiguration(
        assembliesToScanRequestHandlers: Otex.Micros.Applicants.Application.AssemblyReference.Application)
    .AddInfrastructureConfiguration()
    .AddApplicantMicro(connString: "applicants-db");

var app = builder.Build();

app.UseApiConfiguration();

app.Run();