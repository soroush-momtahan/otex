using Aspire.Hosting.Yarp;
using Aspire.Hosting.Yarp.Transforms;
using Otex.Orchestration.Extensions;
using Projects;

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
var applicantApi = builder.AddApplicantsMicroConfiguration(sharedPostgresServer, rabbitmq);

var webGateway = builder.AddYarp("web-gateway")
    .WithConfiguration(yarp =>
    {

        yarp.AddRoute("{**catch-all}", applicantApi)
            .WithOrder(1);
        
        yarp.AddRoute("{**catch-all}", identityApi)
            .WithTransformXForwarded()
            .WithOrder(100);
    });

builder.AddProject<Otex_Clients_Shop_Bff>("shop-bff")
    .WithReference(webGateway)
    .WithReference(identityApi);


builder.Build().Run();