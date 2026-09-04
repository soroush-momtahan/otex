using Projects;

namespace Otex.Orchestration.Extensions;

public static class ApplicantsMicroConfiguration
{
    public static IResourceBuilder<ProjectResource> AddApplicantsMicroConfiguration(
        this IDistributedApplicationBuilder builder,
        IResourceBuilder<PostgresServerResource> sharedPostgresServer,
        IResourceBuilder<RabbitMQServerResource> rabbitMqServer)
    {
        var applicantDb = sharedPostgresServer
            .AddDatabase("applicants-db");

        var applicantsMigrator = builder
            .AddProject<Otex_Micros_Applicants_Migrator>("applicants-migrator")
            .WithReference(applicantDb)
            .WaitFor(applicantDb);

        var applicantsApi = builder
            .AddProject<Otex_Micros_Applicant_Api>("applicants-api")
            .WithReference(applicantDb)
            .WaitFor(applicantDb)
            .WithReference(applicantsMigrator)
            .WaitForCompletion(applicantsMigrator);

        return applicantsApi;
    }
}