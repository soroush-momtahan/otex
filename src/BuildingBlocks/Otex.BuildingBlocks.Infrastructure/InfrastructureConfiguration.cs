using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Otex.BuildingBlocks.Infrastructure.Outbox;

namespace Otex.BuildingBlocks.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IHostApplicationBuilder AddInfrastructureConfiguration(
        this IHostApplicationBuilder builder)
    {
        builder.Services.TryAddSingleton<InsertOutboxMessageInterceptor>();
        return builder;
    }
}
