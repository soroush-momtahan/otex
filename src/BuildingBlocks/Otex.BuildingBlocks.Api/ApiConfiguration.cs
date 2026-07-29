using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Otex.BuildingBlocks.Api.ConfigureOptions.ApiInfoOptions;
using Otex.BuildingBlocks.Api.ConfigureOptions.SwaggerOptions;
using Otex.BuildingBlocks.Api.Extensions;
using Otex.BuildingBlocks.Api.Middlewares;
using Otex.Services.Shared.Presentation.Endpoints;

namespace Otex.BuildingBlocks.Api;

public static class ApiConfiguration
{
    public static IHostApplicationBuilder AddApiConfiguration(
        this IHostApplicationBuilder builder,
        params Assembly[] assembliesToScanEndpoints)
    {
        builder.AddServiceDefaults();
        
        IServiceCollection services = builder.Services;
        
        services.AddSwaggerServices();
        services.AddProblemHandlingServices();
        services.AddEndpoints(assembliesToScanEndpoints);

        // services.AddAuthenticationInternal();
        
        return builder;
    }

    public static IApplicationBuilder UseApiConfiguration(this WebApplication app)
    {
        
        app.UseExceptionHandler();
        app.UseStatusCodePages();
        app.UseHttpsRedirection();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.MapEndpoints();
        return app;
    }

    private static void AddSwaggerServices(this IServiceCollection services)
    {
        
        services.ConfigureOptions<ApiInfoOptionsSetup>();
        services.ConfigureOptions<SwaggerGenOptionsSetup>();
        services.ConfigureOptions<SwaggerUiOptionsSetup>();
        
        // for finding api's by swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    private static void AddProblemHandlingServices(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
    }
}
