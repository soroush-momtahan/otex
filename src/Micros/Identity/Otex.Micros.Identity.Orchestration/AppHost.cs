using Otex.Micros.Identity.Hosting;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresServerResource> sharedPostgresServer =
    builder
        .AddPostgres("postgres")
        .WithDataVolume()
        .WithLifetime(ContainerLifetime.Persistent)
        .WithPgAdmin(resourceBuilder => { resourceBuilder.WithLifetime(ContainerLifetime.Persistent); });

var redis = builder.AddRedis("redis");

IResourceBuilder<RabbitMQServerResource> rabbitmq = builder.AddRabbitMQ("messaging")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithManagementPlugin();

builder.AddIdentityHostingConfiguration<Otex_Micros_Identity_Ui, Otex_Micros_Identity_Migrator>(
    sharedPostgresServer, redis, rabbitmq);

builder.Build().Run();