using Projects;

namespace Otex.Orchestration.Extensions;

public static class IdentityMicroConfiguration
{
    public static IResourceBuilder<ProjectResource> AddIdentityMicroConfiguration(
        this IDistributedApplicationBuilder builder,
        IResourceBuilder<PostgresServerResource> sharedPostgresServer,
        IResourceBuilder<RabbitMQServerResource> rabbitMqServer)
    {
        var identityDb = sharedPostgresServer.AddDatabase("identity-db");
        var identityApi = builder.AddProject<Otex_Micros_Identity_Api>("identity-api");
        
        identityApi
            .WithReference(identityDb)
            .WaitFor(identityDb);
        
        return identityApi;
    }
}