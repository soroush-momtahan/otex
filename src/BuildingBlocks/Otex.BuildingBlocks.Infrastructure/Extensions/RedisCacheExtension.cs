using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Otex.BuildingBlocks.Application.Caching;
using Otex.BuildingBlocks.Infrastructure.Caching;

namespace Otex.BuildingBlocks.Infrastructure.Extensions;

public static class RedisCacheExtension
{
    private static void AddRedisCacheServices(
        this IHostApplicationBuilder builder,
        string cacheName)
    {
        builder.Services.TryAddSingleton<ICacheService, CacheService>();

        builder.AddKeyedRedisDistributedCache(cacheName);
    }
}
