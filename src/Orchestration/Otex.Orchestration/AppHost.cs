using Aspire.Hosting.Yarp;
using Aspire.Hosting.Yarp.Transforms;
using Microsoft.AspNetCore.HttpOverrides;
using Otex.Orchestration.Extensions;
using Projects;
using Yarp.ReverseProxy.Transforms;

var builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresServerResource> sharedPostgresServer =
    builder
        .AddPostgres("postgres")
        .WithDataVolume()
        .WithLifetime(ContainerLifetime.Persistent)
        .WithPgAdmin(resourceBuilder => { resourceBuilder.WithLifetime(ContainerLifetime.Persistent); });

IResourceBuilder<RabbitMQServerResource> rabbitmq = builder.AddRabbitMQ("messaging")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithManagementPlugin();

var identityApi = builder.AddIdentityMicroConfiguration(sharedPostgresServer, rabbitmq);

var webGateway = builder.AddYarp("web-gateway")
    .WithConfiguration(yarp =>
    {
        yarp.AddRoute("{**catch-all}", identityApi)
            .WithTransformXForwarded();
    });

builder.AddProject<Otex_Clients_Shop_Bff>("shop-bff")
    .WithReference(webGateway)
    .WithReference(identityApi);


builder.Build().Run();