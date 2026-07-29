using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Otex.BuildingBlocks.Application.Behaviors;

namespace Otex.BuildingBlocks.Application;

public static class ApplicationConfiguration
{
    public static IHostApplicationBuilder AddApplicationConfiguration(
        this IHostApplicationBuilder builder, params Assembly[] assembliesToScanRequestHandlers)
    {
        IServiceCollection services = builder.Services;
        
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(assembliesToScanRequestHandlers);
            config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
            config.AddOpenBehavior(typeof(ExceptionHandlingPipelineBehavior<,>));
            config.AddOpenBehavior(typeof(RequestLoggingPipelineBehavior<,>));
        });
        services.AddValidatorsFromAssemblies(assembliesToScanRequestHandlers, includeInternalTypes: true);
        return builder;
    }
}
