using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

namespace Otex.Micros.Identity.Hosting;

public static class IdentityHostingConfiguration
{
    public static IResourceBuilder<ProjectResource> AddIdentityHostingConfiguration<TRazor, TMigrator>(
        this IDistributedApplicationBuilder builder,
        IResourceBuilder<PostgresServerResource> sharedPostgresServer,
        IResourceBuilder<RedisResource> redis,
        IResourceBuilder<RabbitMQServerResource> rabbitMqServer)
        where  TRazor: IProjectMetadata, new()
        where  TMigrator: IProjectMetadata, new()
    {
        var identityDb = sharedPostgresServer.AddDatabase("identity-db");
        
        var identityMigrator = builder.AddProject<TMigrator>("identity-migrator")
            .WithReference(identityDb)
            .WaitFor(identityDb);
        
        return builder.AddProject<TRazor>("identity-razor")
            .WithReference(identityDb)
            .WaitFor(identityDb)
            .WithReference(redis)
            .WaitFor(redis)
            .WithReference(identityMigrator)
            .WaitForCompletion(identityMigrator);
    }
}